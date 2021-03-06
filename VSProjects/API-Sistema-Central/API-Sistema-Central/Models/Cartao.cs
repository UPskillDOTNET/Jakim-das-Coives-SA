﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Cartao : Credencial
    {
        public override int Id { get; set; }
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Numero do cartão de crédito inválido")]
        public string Numero { get; set; }
        public string Nome { get; set; }
        [RegularExpression(@"^(0[1-9]|1[0-2])[/]\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")]
        public string DataValidade { get; set; }
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV do cartão de crédito inválido")]
        public string Cvv { get; set; }
        public override int MetodoId { get; set; }

        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
