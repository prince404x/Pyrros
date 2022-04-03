using Microsoft.Extensions.Logging;
using Pyrros.Repository.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Pyrros.Repository.Abstract
{
    public abstract class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        #region variables
        private readonly ILogger _log;
        private ApplicationDbContext _context { get; set; }
        #endregion

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public BaseRepository(ILogger log, ApplicationDbContext context)
        {
            _log = log;
            _context = context;
        }

        /// <summary>
        /// Execute the insert, update and delete statement stored procedures.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns>Returns effected row count.</returns>
        public virtual bool ExecuteSqlCommand(string commandName, object[] sqlParameter)
        {
            bool isSuccess = false;
            int rowCount;
            try
            {
                rowCount = _context.Database.ExecuteSqlRaw(commandName, sqlParameter);
                if (rowCount > 0)
                {
                    isSuccess = true;
                }
            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return isSuccess;
        }

        /// <summary>
        /// Execute the select statement stored procedure.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="sqlParameter"></param>
        /// <returns>Returns a model(s).</returns>
        public virtual IEnumerable<T> ExecuteSqlQuery(string commandName, object[] sqlParameter)
        {
            IEnumerable<T> model = null;
            try
            {
                model = _context.Set<T>().FromSqlRaw(commandName, sqlParameter);
            }
            catch (Exception exception)
            {
                _log.LogError(exception.ToString());
            }
            return model;
        }
    }
}
