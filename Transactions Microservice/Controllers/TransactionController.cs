using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Transactions_Microservice.Helper;
using Transactions_Microservice.Models;
using Transactions_Microservice.Provider;
using Transactions_Microservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transactions_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionController));
        private IProvider _provider;
        Client obj = new Client();
        static int cnt = 1234;
        public TransactionController(IProvider provider)
        {
            _provider = provider;
        }

        // GET: api/<TransactionController>
        [HttpGet("getTransactions")]
        public IActionResult getTransactions(int CustomerId)
        {
            _log4net.Info("GetTransactions Api called");
            if (CustomerId == 0)
            {
                _log4net.Info("Invalid Customer Id");
                return NotFound();
            }
            _log4net.Info("Valid Customer Id");
            List<TransactionHistory> Ts =_provider.GetTransactionHistory(CustomerId);
            if (Ts.Count == 0)
                return NotFound();
            return Ok(Ts);
        }

       /* // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       */

        // POST api/<TransactionController>
        [HttpPost("deposit")]
        public async Task<IActionResult> deposit([FromBody] dynamic model/*int AccountId, int amount*/)
        {
            
            if (Convert.ToInt32(model.AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Either AccountId or amount is invalid");
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });

            }


            HttpClient client = obj.AccountDetails();
            _log4net.Info("getAccount Api called");
            HttpResponseMessage response = client.GetAsync("api/Account/getAccount/?AccountId=" + model.AccountId).Result;

            var result = response.Content.ReadAsStringAsync().Result;
            Account acc = JsonConvert.DeserializeObject<Account>(result);

            _log4net.Info("deposit Api called");
            HttpResponseMessage response1 = client.PostAsJsonAsync("api/Account/deposit",new {AccountId = Convert.ToInt32(model.AccountId), amount = Convert.ToInt32(model.amount) }).Result;
            var result1 = response1.Content.ReadAsStringAsync().Result;
            TransactionStatus st = JsonConvert.DeserializeObject<TransactionStatus>(result1);

            cnt = cnt + 256;
            TransactionHistory history = new TransactionHistory()
            {
                TransactionId = cnt,
                AccountId = Convert.ToInt32(model.AccountId),
                message = st.message,
                source_balance = st.source_balance,
                destination_balance = st.destination_balance,
                DateOfTransaction = DateTime.Now,
                CustomerId = acc.CustomerId
            };
            
            _provider.AddToTransactionHistory(history);

            _log4net.Info("Valid AccountId and amount");
            return Ok(st);
        }

        // POST api/<TransactionController>
        [HttpPost("withdraw")]
        public async Task<IActionResult> withdraw([FromBody] dynamic model/*int AccountId, int amount*/)
        {
            
            if (Convert.ToInt32(model.AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Either AccountId or amount is invalid");
                 return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });
            }
            HttpClient client = obj.AccountDetails();
            _log4net.Info("getAccount Api called");

            HttpResponseMessage response = client.GetAsync("api/Account/getAccount/?AccountId=" + model.AccountId).Result;

            var result = response.Content.ReadAsStringAsync().Result;
            Account acc = JsonConvert.DeserializeObject<Account>(result);

           // HttpClient client1 = obj.RuleApi();
           // var balance = acc.Balance - model.amount;
           // HttpRequestMessage res = client1.GetAsync("api/Rules/evaluateMinBal/?balance=" + balance + "&?AccountId=" + model.AccountId).Result;
            //HttpRequestMessage res = client.GetAsync("api/Rules/evaluateMinBal", new { Balance = Convert.ToInt32(balance),AccountId = Convert.ToInt32(model.AccountId)}).Result;
            // var result2 = response.Content.ReadAsStringAsync().Result;
            // RuleStatus rs = JsonConvert.DeserializeObject<RuleStatus>(result2);

            RuleStatus rs = new RuleStatus() {status = "allowed"};
            if (rs.status == "allowed")
            {
                _log4net.Info("withdraw Api called");
                HttpResponseMessage response1 = client.PostAsJsonAsync("api/Account/withdraw", new { AccountId = Convert.ToInt32(model.AccountId), amount = Convert.ToInt32(model.amount) }).Result;
                var result1 = response1.Content.ReadAsStringAsync().Result;
                TransactionStatus st = JsonConvert.DeserializeObject<TransactionStatus>(result1);

                cnt = cnt + 256;
                TransactionHistory history = new TransactionHistory()
                {
                    TransactionId = cnt,
                    AccountId = Convert.ToInt32(model.AccountId),
                    message = st.message,
                    source_balance = st.source_balance,
                    destination_balance = st.destination_balance,
                    DateOfTransaction = DateTime.Now,
                    CustomerId = acc.CustomerId
                };

                _provider.AddToTransactionHistory(history);

                _log4net.Info("Valid AccountId and amount");
                return Ok(st);
            }
            
            return NotFound(new TransactionStatus() {message = "Withdraw Not Allowed"});
        }

        //POST api/<TransactionController>
        [HttpPost("transfer")]
        public async Task<IActionResult> transfer([FromBody] dynamic model)
        {
            if (Convert.ToInt32(model.Source_AccountId) == 0 || Convert.ToInt32(model.Target_AccountId) == 0  || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Invalid SourceAccountId or TargetAccountId or amount");
                return NotFound(new TransactionStatus() { message = "Transfer Not Allowed" });

            }

            TransactionStatus st = new TransactionStatus();
            st.message = "Transfered from Account no. " + model.Source_AccountId + " To Account no. " + model.Target_AccountId;


            OkObjectResult obj= withdraw(new { AccountId = Convert.ToInt32(model.Source_AccountId), amount = Convert.ToInt32(model.amount) }).Result as OkObjectResult;
            var a = obj.Value as TransactionStatus;
            st.source_balance = a.destination_balance;

            OkObjectResult obj1 = deposit(new { AccountId = Convert.ToInt32(model.Target_AccountId), amount = Convert.ToInt32(model.amount) }).Result as OkObjectResult;
            var a1 = obj1.Value as TransactionStatus;
            st.destination_balance = a1.destination_balance ;

            _log4net.Info("Valid SourceAccountId and TargetAccountId and amount");
            return Ok(st);
        }
    }
}
