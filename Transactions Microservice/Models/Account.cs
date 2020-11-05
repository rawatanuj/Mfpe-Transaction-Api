﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions_Microservice.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public string AccountType { get; set; }
        public int Balance { get; set; }
    }
}
