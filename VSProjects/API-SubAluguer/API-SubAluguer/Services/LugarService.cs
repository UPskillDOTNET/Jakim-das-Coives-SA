﻿using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class LugarService : ILugarService
    {
        private readonly ILugarRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly IReservaRepository _reservaRepository;

        public LugarService(ILugarRepository repository, IParqueRepository parqueRepository, IReservaRepository reservaRepository)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
            _reservaRepository = reservaRepository;
        }

        public async Task<IEnumerable<Lugar>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Lugar> GetByIdAsync(int id)
        {
            Lugar l = await _repository.GetByIdAsync(id);
            if (l == null)
            {
                throw new Exception("Este lugar não existe.");
            }
            return l;
        }

        public async Task<IEnumerable<Lugar>> GetByNifAsync(string nif)
        {
            var temp = await _repository.GetAllAsync();
            var lista = temp.Where(t => t.NifProprietario == nif);
            return lista;
        }

        public async Task<Lugar> PostAsync(Lugar lugar)
        {
            Lugar l = await _repository.PostAsync(lugar);
            if (l == null)
            {
                throw new Exception("O registo do lugar para sub-aluguer falhou.");
            }
            return l;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public IEnumerable<Lugar> FindAvailable(int freguesiaId, DateTime inicio, DateTime fim)
        {
            List<Lugar> todoslugares = new List<Lugar>();
            var parques = _parqueRepository.GetAllAsync().Result.Where(p => p.FreguesiaId == freguesiaId);
            foreach (Parque p in parques)
            {
                var temp = _repository.GetAllAsync().Result.Where(r => r.Inicio <= inicio && r.Fim >= fim && r.ParqueId == p.Id);
                foreach (Lugar l in temp)
                {
                    todoslugares.Add(l);
                }
            }

            List<Lugar> ocupados = new List<Lugar>();
            var reservas = _reservaRepository.GetAllAsync().Result.Where(r => 
                r.Fim > inicio && r.Fim <= fim ||
                r.Inicio >= inicio && r.Inicio < fim ||
                r.Inicio >= inicio && r.Fim <= fim ||
                r.Inicio <= inicio && r.Fim >= fim);
            foreach (Reserva r in reservas)
            {
                ocupados.Add(r.Lugar);
            }

            var disponiveis = todoslugares.Except(ocupados);

            return disponiveis;
        }

        public async Task RemoverUtilizadorAsync(string nif)
        {
            var lugares = await _repository.GetAllAsync();
            foreach (Lugar l in lugares)
            {
                if (l.NifProprietario == nif)
                {
                    l.NifProprietario = "000000000";
                    await _repository.PutAsync(l);
                }
            }
        }
    }
}
