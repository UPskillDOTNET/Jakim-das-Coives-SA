﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;

namespace APP_FrontEnd.Services
{
    public class UtilizadorService : IUtilizadorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public UtilizadorService(IHttpContextAccessor httpContextAccessor, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<double> GetSaldoAsync()
        {

            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNif(nif);

            double saldoUtilizador;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/utilizadores/saldo/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                saldoUtilizador = await response.Content.ReadAsAsync<double>();
            }
            return saldoUtilizador;
        }


        private async Task<string> GetTokenByNif(string nif)
        {
            var user = await _userManager.FindByIdAsync(nif);
            if (user.Expiration < DateTime.UtcNow)
            {
                await _signInManager.SignOutAsync();
                throw new Exception("A sua sessão expirou. Volte a autenticar-se.");
            }
            else
            {
                return user.Token;
            }
        }


        public async Task DepositarSaldoAsync(double valor)
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNif(nif);

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(valor), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/utilizadores/depositar/" + nif;
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
            }
        }


    }
}
