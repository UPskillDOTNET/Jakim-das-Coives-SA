﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class ResetPasswordDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string Nif { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
