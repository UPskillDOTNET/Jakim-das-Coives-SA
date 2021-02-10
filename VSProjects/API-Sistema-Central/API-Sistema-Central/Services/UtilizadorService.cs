﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class UtilizadorService : IUtilizadorService
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IDebitoDiretoRepository _debitoDiretoRepository;
        private readonly IPayPalRepository _payPalRepository;

        public UtilizadorService (UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ICartaoRepository cartaoRepository, IDebitoDiretoRepository debitoDiretoRepository, IPayPalRepository payPalRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartaoRepository = cartaoRepository;
            _debitoDiretoRepository = debitoDiretoRepository;
            _payPalRepository = payPalRepository;
        }

        public async Task<IdentityResult> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            int credencialId;
            switch (registarUtilizadorDTO.MetodoId)
            {
                case 1:
                    Cartao c = new Cartao { MetodoId = registarUtilizadorDTO.MetodoId, Nome = registarUtilizadorDTO.NomeCartao, Numero = registarUtilizadorDTO.Numero, Cvv = registarUtilizadorDTO.Cvv, DataValidade = registarUtilizadorDTO.DataValidade };
                    if (c.Numero == null || c.Nome == null || c.Cvv == null || c.DataValidade == null)
                    {
                        return IdentityResult.Failed();
                    }
                    Cartao cartao = await _cartaoRepository.PostAsync(c);
                    credencialId = cartao.Id;
                    break;
                case 2:
                    DebitoDireto d = new DebitoDireto { MetodoId = registarUtilizadorDTO.MetodoId, Iban = registarUtilizadorDTO.Iban, CodigoPostal = registarUtilizadorDTO.CodigoPostal, Freguesia = registarUtilizadorDTO.Freguesia, Nome = registarUtilizadorDTO.NomeDebitoDireto, DataSubscricao = registarUtilizadorDTO.DataSubscricao, Rua = registarUtilizadorDTO.Rua };
                    if ( d.Iban == null || d.CodigoPostal == null || d.Freguesia == null || d.Rua == null || d.Nome == null)
                    {
                        return IdentityResult.Failed();
                    }
                    DebitoDireto debitoDireto = await _debitoDiretoRepository.PostAsync(d);
                    credencialId = debitoDireto.Id;
                    break;
                case 3:
                    PayPal p = new PayPal { MetodoId = registarUtilizadorDTO.MetodoId, Email = registarUtilizadorDTO.EmailPayPal, Password = registarUtilizadorDTO.PasswordPayPal };
                    if (p.Password == null || p.Email == null)
                    {
                        return IdentityResult.Failed();
                    }
                    PayPal payPal = await _payPalRepository.PostAsync(p);
                    credencialId = payPal.Id;
                    break;
                default:
                    return IdentityResult.Failed();
            }

            var user = new Utilizador { Id = registarUtilizadorDTO.Nif, UserName = registarUtilizadorDTO.EmailUtilizador, Nome = registarUtilizadorDTO.NomeUtilizador, Email = registarUtilizadorDTO.EmailUtilizador, CredencialId = credencialId };
            var result = await _userManager.CreateAsync(user, registarUtilizadorDTO.PasswordUtilizador);
            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(infoUtilizadorDTO.Email, infoUtilizadorDTO.Password, isPersistent: false, lockoutOnFailure: false);
            return result;
        }
    }
}