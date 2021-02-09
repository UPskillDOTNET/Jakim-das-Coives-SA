﻿using API_SubAluguer.Data;
using API_SubAluguer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Repository
{
    public class DisponibilidadeRepository : RepositoryBase<Disponibilidade>, IDisponibilidadeRepository
    {
        public DisponibilidadeRepository(API_SubAluguerContext context) : base(context)
        {
        }
    }
}
