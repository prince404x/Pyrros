using Microsoft.Extensions.Logging;
using Pyrros.Entity.Model;
using Pyrros.Repository;
using Pyrros.Repository.DataContext;

namespace Pyrros.Service
{
    public class WalletService
    {
        #region variables
        private readonly ILogger _log;
        private readonly ApplicationDbContext _context;
        IWalletRepository walletRepository = null;
        Wallet wallet = null;
        #endregion

        public WalletService(ILogger log, ApplicationDbContext context)
        {
            _log = log;
            _context = context;
            walletRepository = new WalletRepository(_log, _context);
        }


        /// <summary>
        /// Get wallet from repository.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Wallet GetWallet(int account)
        {
            try
            {
                wallet = walletRepository.GetWallet(account);
            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return wallet;
        }
    }
}
