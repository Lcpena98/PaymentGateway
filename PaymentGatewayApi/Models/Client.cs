namespace PaymentGatewayApi.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}