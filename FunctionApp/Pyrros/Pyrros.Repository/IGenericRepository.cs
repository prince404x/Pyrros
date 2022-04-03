namespace Pyrros.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        bool ExecuteSqlCommand(string commandName, object[] sqlParameter);
        IEnumerable<T> ExecuteSqlQuery(string commandName, object[] sqlParameter);
    }
}
