using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pyrros.Entity.Model;
using Pyrros.Service;
using Pyrros.Repository.DataContext;

namespace Pyrros.FunctionApp
{
    public class AddTransaction
    {
        #region variables
        TransactionService transactionService = null;
        private readonly ApplicationDbContext _context;
        Status status = null;
        #endregion

        public AddTransaction(ApplicationDbContext context)
        {
            _context = context;
        }

        [FunctionName("AddTransaction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Transaction transaction = JsonConvert.DeserializeObject<Transaction>(requestBody);
                if (transaction != null)
                {
                    AddLog(log, transaction);
                    if (IsValidTransaction(transaction))
                    {
                        transactionService = new TransactionService(log, _context);
                        status = transactionService.AddTransaction(transaction);
                    }
                }
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                status = new Status { Type = false, Response = "Invalid transaction." };
            }
            return new OkObjectResult(status);
        }

        /// <summary>
        /// Log the transaction details.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="transaction"></param>
        private void AddLog(ILogger log, Transaction transaction)
        {
            Log _log = new Log { FunctionName = "AddTransaction" };
            _log.Parameter.Add("Transaction", transaction);
            log.LogInformation(JsonConvert.SerializeObject(_log));
        }

        /// <summary>
        /// If transaction values are invalid then return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private bool IsValidTransaction(Transaction transaction)
        {
            bool isValid = false;
            if (transaction.Id > 0)
            {
                if (transaction.Amount > 0)
                {
                    if (!string.IsNullOrEmpty(transaction.Direction))
                    {
                        if (transaction.Account > 0)
                        {
                            isValid = true;
                        }
                        else
                            status = new Status { Type = false, Response = "Invalid transaction. Account should be greater than 0." };
                    }
                    else
                        status = new Status { Type = false, Response = "Invalid transaction. Direction have some value." };
                }
                else
                    status = new Status { Type = false, Response = "Invalid transaction. Amount should be greater than 0." };
            }
            else
                status = new Status { Type = false, Response = "Invalid transaction. Id should be greater than 0." };
            return isValid;
        }
    }
}
