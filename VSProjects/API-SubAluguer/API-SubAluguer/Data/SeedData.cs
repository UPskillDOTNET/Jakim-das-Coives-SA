﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_SubAluguer.Models;
using API_SubAluguer.Controllers;

namespace API_SubAluguer.Data
{
    public class SeedData
    {
        public static void Initialize(API_SubAluguerContext context)
        {
            // Look for any Cliente.
            if (context.Clientes.Any())
            {
                return;   // DB was already seeded
            }

            context.Clientes.AddRange(
                new Cliente { Nif = 111222333, Nome = "Sistema Central", Email = "jakimdascoives@upskill.pt" },
                new Cliente { Nif = 222222222, Nome = "Fake Client 1", Email = "fakeclient1@upskill.pt" },
                new Cliente { Nif = 333333333, Nome = "Fake Client 2", Email = "fakeclient2@upskill.pt" },
                new Cliente { Nif = 444444444, Nome = "Fake Client 3", Email = "fakeclient3@upskill.pt" },
                new Cliente { Nif = 555555555, Nome = "Fake Client 4", Email = "fakeclient4@upskill.pt" }
            );
            context.SaveChanges();

            context.Freguesias.AddRange(
                new Freguesia { Nome = "Porto" }
            );
            context.SaveChanges();

            context.Parques.AddRange(
                new Parque { Rua = "Rua do parque privado 3", FreguesiaId = 1 }
            );
            context.SaveChanges();

            context.Lugares.AddRange(
                new Lugar { Numero = 1, Fila = "A", Andar = -2, ParqueId = 1, Preco = 5.99 },
                new Lugar { Numero = 2, Fila = "B", Andar = -1, ParqueId = 1, Preco = 4.99 },
                new Lugar { Numero = 3, Fila = "C", Andar = 0, ParqueId = 1, Preco = 5.49 },
                new Lugar { Numero = 4, Fila = "D", Andar = 1, ParqueId = 1, Preco = 4.89 },
                new Lugar { Numero = 5, Fila = "E", Andar = 2, ParqueId = 1, Preco = 5.29 }
            );
            context.SaveChanges();

            context.Reservas.AddRange(
                new Reserva { NifCliente = 222222222, LugarId = 1, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 12:00:00") },
                new Reserva { NifCliente = 333333333, LugarId = 2, Inicio = DateTime.Parse("2021-01-29 21:00:00"), Fim = DateTime.Parse("2021-01-31 10:00:00") },
                new Reserva { NifCliente = 444444444, LugarId = 3, Inicio = DateTime.Parse("2021-01-30 10:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") },
                new Reserva { NifCliente = 555555555, LugarId = 4, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") }
            );
            context.SaveChanges();
        }
    }
}