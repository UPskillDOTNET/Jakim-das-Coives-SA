﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJakim.Pages;

namespace TesteJakim.Tests
{
    class LoginTest
    {
        //Abrir Browser (Se n abrir confirmar versao do browser)
        IWebDriver webDriver = new ChromeDriver(/*versao do browser*/);

        [SetUp]
        public void Setup()
        {
            //Entrar no Site
            webDriver.Navigate().GoToUrl("https://localhost:5055/");

        }

        [Test]
        public void Login()
        {
            HomePage homePage = new HomePage(webDriver);
            homePage.ClickLogin();

            LoginPage loginPage = new LoginPage(webDriver);
            loginPage.Login("sistemacentraljakim@gmail.com", "123Pa$$word");

            Assert.That(homePage.IncertUserLogin, Is.True);

        }
    }
}
