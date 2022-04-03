namespace Pyrros.Entity.Model
{
    public class Log
    {
        public string FunctionName { get; set; }
        public Dictionary<string, object> Parameter { get; set; }

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        public Log()
        {
            this.Parameter = new Dictionary<string, object>();
        }
    }
}
