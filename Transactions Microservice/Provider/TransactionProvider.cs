using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Models;
using Transactions_Microservice.Repository;

namespace Transactions_Microservice.Provider
{
    public class TransactionProvider : IProvider
    {
        private IRepository _repo;
        public TransactionProvider(IRepository repo)
        {
            _repo = repo;
        }
        public bool AddToTransactionHistory(TransactionHistory history)
        {
            bool output = _repo.AddToTransactionHistory(history);
            if (output)
            {
                return true;
            }
            return false;
        }

        public List<TransactionHistory> GetTransactionHistory(int CustomerId)
        {
            List<TransactionHistory> list = _repo.GetTransactionHistory(CustomerId);
            if(list.Count == 0)
            {
                return null;
            }
            return list;
        }
    }
}
