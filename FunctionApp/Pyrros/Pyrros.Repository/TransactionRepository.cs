using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Pyrros.Entity.Model;
using Pyrros.Repository.Abstract;
using Pyrros.Repository.DataContext;

namespace Pyrros.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        #region variables
        private readonly ILogger _log;
        private SqlParameter[] _sqlParameter = null;
        #endregion

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        public TransactionRepository(ILogger log, ApplicationDbContext context) : base(log, context)
        {
            _log = log;
        }

        /// <summary>
        /// Add transaction to database.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool AddTransaction(Transaction transaction)
        {
            bool isAdd = false;
            try
            {
                _sqlParameter = new SqlParameter[4];
                _sqlParameter[0] = new SqlParameter("@id", transaction.Id);
                _sqlParameter[1] = new SqlParameter("@amount", transaction.Amount);
                _sqlParameter[2] = new SqlParameter("@direction", transaction.Direction);
                _sqlParameter[3] = new SqlParameter("@account", transaction.Account);
                isAdd = ExecuteSqlCommand("exec UspInsertTransaction @id, @amount, @direction, @account", _sqlParameter);
            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return isAdd;
        }
    }
}