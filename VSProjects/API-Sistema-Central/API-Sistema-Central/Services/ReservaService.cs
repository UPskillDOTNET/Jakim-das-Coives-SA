﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Sistema_Central.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<Utilizador> _userManager;
        private readonly IPagamentoService _payment;

        public ReservaService(IReservaRepository repository, IParqueRepository parqueRepository, ITransacaoRepository transacaoRepository, UserManager<Utilizador> userManager, IPagamentoService paymentService, IEmailService emailService)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
            _transacaoRepository = transacaoRepository;
            _userManager = userManager;
            _emailService = emailService;
            _payment = paymentService;
        }

        public async Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            var listaLugares = new List<LugarDTO>();
            var listaParques = _parqueRepository.GetAllAsync().Result;
            foreach (Parque parque in listaParques.Value)
            {
                var f = GetFreguesiaByNome(freguesiaNome, parque.ApiUrl).Result;
                if (f != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string endpoint1 = parque.ApiUrl + "api/lugares/disponibilidade/" + f.Id + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + " / " + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                        var response1 = await client.GetAsync(endpoint1);
                        response1.EnsureSuccessStatusCode();
                        List<LugarDTO> temp = await response1.Content.ReadAsAsync<List<LugarDTO>>();
                        foreach (LugarDTO l in temp)
                        {
                            l.ParqueIdSC = parque.Id;
                            l.ApiUrl = parque.ApiUrl;
                            listaLugares.Add(l);
                        }
                    }
                };
            }
            return listaLugares;   
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetByNifAsync(string nif)
        {
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifUtilizador == nif);
            return lista.ToList();
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Reserva> PostAsync(ReservaDTO reservaDTO)
        {
            Reserva reserva = new Reserva { NifUtilizador = reservaDTO.NifUtilizador, ParqueId = reservaDTO.ParqueId };

            //Reservar o lugar na API-Parque
            ReservaAPIParqueDTO r = new ReservaAPIParqueDTO { NifCliente = 999999999, LugarId = reservaDTO.LugarId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim };
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
                string endpoint = reservaDTO.ApiUrl + "api/reservas";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                ReservaAPIParqueDTO r2 = await response.Content.ReadAsAsync<ReservaAPIParqueDTO>();
                reserva.ReservaParqueId = r2.Id;
            }
            
            //Calcular o custo da Reserva
            var h = (reservaDTO.Fim - reservaDTO.Inicio).TotalHours;
            reserva.Custo = h * reservaDTO.Preco;

            //Fazer pagamento da reserva
            Utilizador utilizador = await _userManager.FindByIdAsync(reservaDTO.NifUtilizador);

            PagamentoDTO payDTO = new PagamentoDTO { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, MetodoId = reservaDTO.MetodoId, Valor = reserva.Custo};

            await _payment.Pay(payDTO);

            //Registar a transacao do pagamento da reserva
            Transacao transacao = await _transacaoRepository.PostAsync(new Transacao { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo, MetodoId = reservaDTO.MetodoId, DataHora = DateTime.UtcNow });
            reserva.TransacaoId = transacao.Id;

            //Enviar email de confirmacao
            var f = GetFreguesiaNomeByParqueID(reservaDTO.ParqueId, reservaDTO.ApiUrl).Result;
            var p = GetParqueNomeByID(reservaDTO.ParqueId, reservaDTO.ApiUrl).Result;
            var l = GetLugarByID(reservaDTO.LugarId, reservaDTO.ApiUrl).Result;
            QRCodeDTO qr = new QRCodeDTO { NomeUtilizador = utilizador.Nome, Email = utilizador.Email, IdReserva = reserva.ReservaParqueId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim, NomeFreguesia = f, NomeParque = p, NumeroLugar = l.Numero, Fila = l.Fila, Andar = l.Andar };
            _emailService.EnviarEmail(qr);

            return await _repository.PostAsync(reserva);
        }

        public async Task DeleteAsync(int id)
        {
            Reserva reserva = await _repository.GetByIdAsync(id);
            Parque parque = await _parqueRepository.GetByIdAsync(reserva.ParqueId);

            //Apagar a reserva na API-Parque
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = parque.ApiUrl + "api/reservas/" + reserva.ReservaParqueId;
                var response1 = await client.DeleteAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
            }

            //Reembolsar a carteira do utilizador

            await _repository.DeleteAsync(id);
        }

        private async Task<string> GetFreguesiaNomeByParqueID(int id, string url)
        {
            FreguesiaDTO f;
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = url + "api/parques/" + id;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                ParqueDTO p = await response1.Content.ReadAsAsync<ParqueDTO>();

                string endpoint2 = url + "api/freguesias/" + p.FreguesiaId;
                var response2 = await client.GetAsync(endpoint2);
                response2.EnsureSuccessStatusCode();
                f = await response2.Content.ReadAsAsync<FreguesiaDTO>();
            }
            return f.Nome;
        }
        private async Task<string> GetParqueNomeByID(int id, string url)
        {
            ParqueDTO p;
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = url + "api/parques/" + id;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                p = await response1.Content.ReadAsAsync<ParqueDTO>();
            }
            return p.Rua;
        }
        private async Task<LugarDTO> GetLugarByID(int id, string url)
        {
            LugarDTO l;
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = url + "api/lugares/" + id;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                l = await response1.Content.ReadAsAsync<LugarDTO>();
            }
            return l;
        }
        private async Task<FreguesiaDTO> GetFreguesiaByNome(string nome, string url)
        {
            FreguesiaDTO f;
            using (HttpClient client = new HttpClient())
            {
                var listaFreguesias = new List<FreguesiaDTO>();
                string endpoint1 = url + "api/freguesias";
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                listaFreguesias = await response1.Content.ReadAsAsync<List<FreguesiaDTO>>();
                f = listaFreguesias.Find(z => z.Nome == nome);
            }
            return f;
        }
    }
}