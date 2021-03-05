﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace APP_FrontEnd.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public IndexModel(
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [Display(Name = "NIF")]
        public string Nif { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Nome completo")]
            public string Name { get; set; }
            [Phone]
            [Display(Name = "Número de telefone")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Utilizador user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Nif = user.Id;

            Input = new InputModel
            {
                Name = user.Nome,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Name != user.Nome)
            {
                string nomeActual = user.Nome;
                user.Nome = Input.Name;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await AlterarNomeAsync(new AlterarNomeDTO { Nif = user.Id, NomeActual = nomeActual, NomeNovo = Input.Name }, user.Token);
                }
                else
                {
                    StatusMessage = "Erro inesperado ocorreu aquando do registo do nome completo.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ocorreu aquando do registo do número de telemóvel.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "O seu perfil foi atualizado";
            return RedirectToPage();
        }
        private async Task AlterarNomeAsync(AlterarNomeDTO alterarNomeDTO, string token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(alterarNomeDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/utilizadores/nome";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("Alteração de nome falhou no servidor. Volte a tentar.");
            }
        }
    }
}
