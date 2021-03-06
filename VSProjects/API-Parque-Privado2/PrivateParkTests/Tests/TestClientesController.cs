﻿using API_Parque_Privado2.Controllers;
using API_Parque_Privado2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Unit_Tests
{
    public class TestClientesController
    {
        [Fact]
        public async Task GetAllClientesAsync_ShouldReturnAllClientsAsync() { // retornar todos os clientes da base de dados
            //Arrange

            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBGetAllClients");
            var TestController = new ClientesController(TestContext);

            //Act

            var result = await TestController.GetCliente();

            //Assert

            var items = Assert.IsType<List<Cliente>>(result.Value);
            Assert.Equal(6, items.Count);
        }

        [Fact]
        public async Task GetClientesByNifAsync_ShouldReturnManelDosPCs() // deverá retornar o cliente "João dos PC's"
        {
            //Arrange

            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBGetClienteByNif");
            var TestController = new ClientesController(TestContext);

            //Act

            var result = await TestController.GetCliente(123456789);

            //Assert

            var item = Assert.IsType<Cliente>(result.Value);
            Assert.Equal("Joao", item.Nome);
            Assert.Equal("joao@upskill.pt", item.Email);
        }

        [Fact]
        public async Task PostClienteAsync_ShouldReturnCreatedAsync() // criar o cliente "João do Hardware"
        {
            //Arrange

            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBPostCliente");
            var TestController = new ClientesController(TestContext);


            //Act

            var result = await TestController.PostCliente(new Cliente { Nif = 999888777, Nome = "João do Hardware", Email = "joaodohardware@clix.pt" });
            var get = await TestController.GetCliente(999888777);

            //Assert

            var item = Assert.IsType<Cliente>(get.Value);
            Assert.Equal("João do Hardware", item.Nome);
            Assert.Equal("joaodohardware@clix.pt", item.Email);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PutClienteAsync_ShouldUpdateClientSistemaCentralAsync() // atualizar o cliente Sistema Central
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBPutCliente");
            var TestController = new ClientesController(TestContext);

            //Act
            var client = new Cliente
            {
                Nif = 111222333,
                Nome = "Central System",
                Email = "centralsystem@jakimdascoives.us"
            };
            var getClient = await TestController.GetCliente(111222333);
            var c1 = getClient.Value;
            TestContext.Entry(c1).State = EntityState.Detached;
            var result = await TestController.PutCliente(111222333, client);
            var getresult = await TestController.GetCliente(111222333);

            //Assert
            Assert.IsType<NoContentResult>(result);
            var item = Assert.IsType<Cliente>(getresult.Value);
            Assert.Equal(111222333, item.Nif);
            Assert.Equal("Central System", item.Nome);
            Assert.Equal("centralsystem@jakimdascoives.us", item.Email);
        }

        [Fact]
        public async Task DeleteClienteAsync_ShouldDeleteClienteSistemaCentralAsync() // apagar o Sistema Central
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBDeleteCliente");
            var TestController = new ClientesController(TestContext);

            //Act
            var result = await TestController.DeleteCliente(111222333);
            var get = await TestController.GetCliente(111222333);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
