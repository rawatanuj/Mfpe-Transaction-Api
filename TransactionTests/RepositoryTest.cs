using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Transactions_Microservice.Models;
using Transactions_Microservice.Repository;

namespace TransactionTests
{
    [TestFixture]
    public class RepositoryTest
    {
      private TransactionRepository _transactionrepo;
      
      [SetUp]
      public void Setup()
      {
       _transactionrepo = new TransactionRepository();

      }

        [Test]
        public void AddToTransactionHistory_WhenCalled_Returnstrue()
        {
         var result = _transactionrepo.AddToTransactionHistory(new TransactionHistory());

         Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void AddToTransactionHistory_WhenCalled_ReturnsFalse()
        {
            var result = _transactionrepo.AddToTransactionHistory(null);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void GetTransactionHistory_WhenCalled_ReturnsNull()
        { 

            var result = _transactionrepo.GetTransactionHistory(-1);

            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
