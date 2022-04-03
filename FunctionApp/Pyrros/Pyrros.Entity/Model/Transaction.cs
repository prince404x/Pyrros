namespace Pyrros.Entity.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Direction { get; set; }
        public int Account { get; set; }
    }
}
