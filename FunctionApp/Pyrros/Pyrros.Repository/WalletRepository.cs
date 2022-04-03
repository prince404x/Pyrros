using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Pyrros.Entity.Model;
using Pyrros.Repository.Abstract;
using Pyrros.Repository.DataContext;

namespace Pyrros.Repository
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
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
        public WalletRepository(ILogger log, ApplicationDbContext context) : base(log, context)
        {
            _log = log;
        }

        /// <summary>
        /// Get wallet from database.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Wallet GetWallet(int account)
        {
            Wallet wallet = null;
            _sqlParameter = new SqlParameter[1];
            try
            {
                _sqlParameter[0] = new SqlParameter("@account", account);
                wallet = ExecuteSqlQuery("exec UspSelectWallet @account", _sqlParameter).FirstOrDefault();
            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return wallet;
        }
    }
}
