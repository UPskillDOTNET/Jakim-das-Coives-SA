﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class DetalheReservaDTO
    {
        public int ReservaId { get; set; }
        public double Custo { get; set; }
        public int ReservaParqueId { get; set; }
        public string NomeFreguesia { get; set; }
        public string NomeParque { get; set; }
        public int NumeroLugar { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        public bool IsSubAlugado { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
    }
}
