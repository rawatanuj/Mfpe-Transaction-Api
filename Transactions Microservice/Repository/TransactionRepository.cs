using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Models;

namespace Transactions_Microservice.Repository
{
    public class TransactionRepository : IRepository
    {
        static List<TransactionHistory> status = new List<TransactionHistory>();
        List<TransactionHistory> status1 = new List<TransactionHistory>();
        public bool AddToTransactionHistory(TransactionHistory history)
        {
            if (history != null)
            {
                status.Add(history);
                return true;
            }
            return false;
        }

        public List<TransactionHistory> GetTransactionHistory(int CustomerId)
        {
            
            foreach(var list in status)
            {
                if (list.CustomerId == CustomerId)
                {
                    status1.Add(list);
                }
            }
            
            return status1;
        }
    }
}
