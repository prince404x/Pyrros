using Pyrros.Entity.Model;

namespace Pyrros.Repository
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Add transaction to database.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        bool AddTransaction(Transaction transaction);
    }
}
