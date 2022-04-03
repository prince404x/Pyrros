using Pyrros.Entity.Model;

namespace Pyrros.Repository
{
    public interface IWalletRepository
    {
        /// <summary>
        /// Get wallet from database.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Wallet GetWallet(int account);
    }
}
