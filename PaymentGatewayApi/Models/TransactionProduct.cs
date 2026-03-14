namespace PaymentGatewayApi.Models
{
    public class TransactionProduct
    {
        public int TransactionId { get; set; }
        public Transaction transaction { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int Quantity { get; set; }
    }
}
