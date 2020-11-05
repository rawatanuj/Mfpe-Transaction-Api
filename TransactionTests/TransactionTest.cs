using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Transactions_Microservice.Controllers;
using Transactions_Microservice.Helper;
using Transactions_Microservice.Models;
using Transactions_Microservice.Provider;

namespace TransactionTests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IProvider> _provider;
        private TransactionController _controller;
        
        [SetUp]
        public void Setup()
        {
            _provider = new Mock<IProvider>();
            _controller = new TransactionController(_provider.Object);
        }

        [Test]
        public void getTransactions_WhenCalled_ReturnsOk()
        {
            _provider.Setup(repo => repo.GetTransactionHistory(It.IsAny<int>())).Returns(new List<TransactionHistory> { new TransactionHistory() });
            
            var result = _controller.getTransactions(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void getTransactions_WhenCalledWithZero_ReturnsNotFound()
        {
            //_provider.Setup(repo => repo.GetTransactionHistory(It.IsAny<int>())).Returns(new List<TransactionHistory> { new TransactionHistory() });

            var result = _controller.getTransactions(0);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }


        [Test]
        public void getTransactions_WhenCalledAndIDIsNotInDB_ReturnsNotFound()
        {
            _provider.Setup(repo => repo.GetTransactionHistory(It.IsAny<int>())).Returns(new List<TransactionHistory> { });

            var result = _controller.getTransactions(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void deposit_WhenCalledWithZeroValues_ReturnsNotFoundOk()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.deposit(new Obj{ AccountId = 0, amount = 0 }).Result;

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void deposit_WhenCalledWithNonZeroValues_ReturnsOk()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.deposit(new Obj { AccountId = 1, amount = 1 }).Result;

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void deposit_WhenCalledWithNonZeroValues_ReturnsNotFound()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.deposit(new Obj { AccountId = 0, amount = 0 }).Result;

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void withdraw_WhenCalledWithZeroValues_Ok()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.withdraw(new Obj { AccountId = 1, amount = 1 }).Result;

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void withdraw_WhenCalledWithZeroValues_ReturnsNotFoundOk()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.withdraw(new Obj { AccountId = 0, amount = 0 }).Result;

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Transfer_WhenCalledWithZeroValues_ReturnsOk()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.transfer(new Obj2 { Source_AccountId = 1, Target_AccountId = 1, amount = 1 }).Result;

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Transfer_WhenCalledWithZeroValues_ReturnsNotFoundOk()
        {
            _provider.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionHistory>())).Verifiable();

            var result = _controller.transfer(new Obj2 { Source_AccountId = 0, Target_AccountId = 0, amount = 0 }).Result;

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}