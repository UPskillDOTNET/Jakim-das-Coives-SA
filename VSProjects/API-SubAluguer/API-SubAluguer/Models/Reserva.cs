﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int NifCliente { get; set; }
        public int LugarId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }


        [ForeignKey("NifCliente")]
        public Cliente Cliente { get; set; }
        [ForeignKey("LugarId")]
        public Lugar Lugar { get; set; }
    }
}
