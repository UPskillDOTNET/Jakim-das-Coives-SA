﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API_Parque_Privado2.Repositories
{
    public interface IRepositoryBase<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task PutAsync(T entity);
        public Task<T> PostAsync(T entity);
        public Task DeleteAsync(int id);
    }
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected API_Parque_Privado2Context _context;

        public RepositoryBase(API_Parque_Privado2Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }
        public async Task PutAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<T> PostAsync(T entity)
        {
            EntityEntry<T> entry;
            entry = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
