using ConversorMoedas;
using ConversorMoedas.Controllers;
using ConversorMoedas.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConversorMoedas.Tests.Controllers
{
    [TestClass]
    public class TransactionsControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Organizar
            var controller = new TransactionsController();  
            var result = controller.Index() as ViewResult; 
            var list = (List<Transaction>)result.ViewData.Model;
            // Declarar 
            Assert.IsNotNull(list);
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod]
        public void Details()
        {
            int IdTransactionTest = 1002;
            // Organizar
            var controller = new TransactionsController(); 
            // Agir
            var result = controller.Details(IdTransactionTest) as ViewResult;
            var transaction = (Transaction)result.ViewData.Model;

            // Declarar
            Assert.IsNotNull(transaction);
            Assert.AreEqual(transaction.IdTransaction, IdTransactionTest);
        }

        [TestMethod]
        public void Create()
        {
            // Organizar
            var controller = new TransactionsController();
            var result = controller.Create() as ViewResult;
            var vBag = result.ViewBag;
            
            // Declarar 
            Assert.IsNotNull(vBag);
            Assert.AreNotEqual(0, vBag.Count);
        }
    }
}
