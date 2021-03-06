﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class DebitoDireto : Credencial
    {
        public override int Id { get; set; }
        [RegularExpression(@"^[P][T][5][0]\d{21}$", ErrorMessage = "IBAN inválido")]
        public string Iban { get; set; }
        public string Nome { get; set; }
        public string Rua { get; set; }
        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "Código postal inválido")]
        public string CodigoPostal { get; set; }
        public string Freguesia { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DataSubscricao { get ; set; }
        public override int MetodoId { get; set; }

        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
