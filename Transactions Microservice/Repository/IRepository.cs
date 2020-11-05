using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Models;

namespace Transactions_Microservice.Repository
{
    public interface IRepository
    {
       bool AddToTransactionHistory(TransactionHistory history);
       List<TransactionHistory> GetTransactionHistory(int CustomerId);
    }
}
