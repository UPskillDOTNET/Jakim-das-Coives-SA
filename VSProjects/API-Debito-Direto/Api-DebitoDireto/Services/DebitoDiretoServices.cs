﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_DebitoDireto.Models;
using Api_DebitoDireto.Controllers;
using Api_DebitoDireto.Repositories;


namespace Api_DebitoDireto.Services
{
    public interface IDebitoDiretoService
    {
        public Task<ActionResult<IEnumerable<DebitoDireto>>> GetAllDebitoDireto();
        public Task<DebitoDireto> GetByIdAsync(int id);
        public Task<DebitoDireto> PostDebitoDireto(DebitoDireto debitoDireto);
    }
    public class DebitoDiretoServices : IDebitoDiretoService
    {
        private readonly IDebitoDiretoRepository _repository;

        public DebitoDiretoServices(IDebitoDiretoRepository repository)
        {
            _repository = repository;
        }
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> GetAllDebitoDireto()
        {
            return await _repository.ReturnAllDebitoDireto();
        }
        public async Task<DebitoDireto> GetByIdAsync(int id)
        {
            var d = await _repository.GetByIdAsync(id);
            if (d == null)
            {
                throw new Exception("O débito direto solicitado não existe.");
            }
            return d;
        }
        public async Task<DebitoDireto> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            return await _repository.PostDebitoDireto(debitoDireto);
        }
    }
}
