﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public interface IReservaRepository : IRepositoryBase<Reserva>
    {
    }
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        public ReservaRepository(SCContext context) : base(context)
        {
        }
    }
}
