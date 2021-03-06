﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APP_FrontEnd.Models
{
    public class SubAluguerDTO
    {
        [Display(Name = "Nº de Sub-Aluguer")]
        public int Id { get; set; }
        public int ParqueId { get; set; }

        [Display(Name = "Parque")]
        public string NomeParque { get; set; }

        [Display(Name = "Nº de Lugar")]
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        [DataType(DataType.Currency)]

        [Display(Name = "Preço por hora")]
        public double Preco { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifProprietario { get; set; }
        public int ReservaSistemaCentralId { get; set; }

        [Display(Name = "Encontra-se Reservado?")]
        public bool IsReservado { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Início")]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Fim")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
    }
}
