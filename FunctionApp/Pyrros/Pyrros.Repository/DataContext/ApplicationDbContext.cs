using Microsoft.EntityFrameworkCore;
using Pyrros.Entity.Model;

namespace Pyrros.Repository.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
    }
}
