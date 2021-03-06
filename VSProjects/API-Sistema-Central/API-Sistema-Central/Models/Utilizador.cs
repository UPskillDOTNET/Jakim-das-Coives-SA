﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API_Sistema_Central.Authentication;
using Microsoft.AspNetCore.Identity;

namespace API_Sistema_Central.Models
{
    public class Utilizador : IdentityUser
    {
        public string Nome { get; set; }
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "1000000")]
        public double Carteira { get; set; }
        public int CredencialId { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }

        [ForeignKey("CredencialId")]
        public Credencial Credencial { get; set; }
    }
}
