using System.Text.Json.Serialization;
using System.Transactions;

namespace PaymentGatewayApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string Gateway { get; set; }
        public string ExternalId { get; set; }
        public TransactionStatus Status { get; set; }
        public int Amount { get; set; }
        public string CardLastNumbers { get; set; }
        public ICollection<TransactionProduct> Products { get; set; }
    }
}