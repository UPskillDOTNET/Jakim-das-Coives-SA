﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public abstract class Credencial
    {
        public abstract int Id { get; set; }
        public abstract int MetodoId { get; set; }
    }
}
