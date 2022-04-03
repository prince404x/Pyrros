using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pyrros.Entity.Model;
using Pyrros.Repository;
using Pyrros.Repository.DataContext;

namespace Pyrros.Service
{
    public class TransactionService
    {
        #region variables
        private readonly ILogger _log;
        private readonly ApplicationDbContext _context;
        ITransactionRepository transactionRepository = null;
        WalletService walletService = null;
        Status status = null;
        Wallet wallet = null;
        #endregion

        public TransactionService(ILogger log, ApplicationDbContext context)
        {
            _log = log;
            _context = context;
            transactionRepository = new TransactionRepository(_log, _context);
            walletService = new WalletService(_log, _context);
        }

        /// <summary>
        /// Check for all the business logic if its valid then add transaction to the database. Otherwise it return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Status AddTransaction(Transaction transaction)
        {
            bool isAdd = false;
            string direction = string.Empty;
            try
            {
                direction = transaction.Direction.ToLower();
                isAdd = IsValidDirection(transaction, isAdd, direction);

            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return status;
        }

        /// <summary>
        /// If its a valid direction then it check for valid account. Otherwise it return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="isAdd"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool IsValidDirection(Transaction transaction, bool isAdd, string direction)
        {
            if (direction == "credit" || direction == "debit")
            {
                wallet = walletService.GetWallet(transaction.Account);
                isAdd = IsValidAccount(transaction, isAdd, direction);
            }
            else
                status = new Status { Type = false, Response = "Invalid transaction. Direction should be either Credit or Debit." };
            return isAdd;
        }

        /// <summary>
        /// If the account is valid then it check for sufficient balance. Otherwise it return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="isAdd"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool IsValidAccount(Transaction transaction, bool isAdd, string direction)
        {
            if (wallet != null)
            {
                isAdd = IsInsufficientBalance(transaction, isAdd, direction);
            }
            else
                status = new Status { Type = false, Response = "Invalid transaction. Account is not valid." };
            return isAdd;
        }

        /// <summary>
        /// If there is sufficient balance or the direction is credit then it will add the transcation. Otherwise it return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="isAdd"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool IsInsufficientBalance(Transaction transaction, bool isAdd, string direction)
        {
            if ((direction == "debit" && wallet.Balance >= transaction.Amount) || direction == "credit")
            {
                isAdd = transactionRepository.AddTransaction(transaction);
                IsAdd(transaction, isAdd, direction);
            }
            else
                status = new Status { Type = false, Response = "Invalid transaction. Account has insufficient balance." };
            return isAdd;
        }

        /// <summary>
        /// If transaction is added then update the wallet balance and return it. Otherwise it return error message.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="isAdd"></param>
        /// <param name="direction"></param>
        private void IsAdd(Transaction transaction, bool isAdd, string direction)
        {
            if (isAdd)
            {
                wallet.Balance = direction == "debit" ? wallet.Balance - transaction.Amount : wallet.Balance + transaction.Amount;
                status = new Status { Type = true, Response = JsonConvert.SerializeObject(wallet) };
            }
            else
                status = new Status { Type = false, Response = "Something went wrong. Please try after sometime." };
        }
    }
}
