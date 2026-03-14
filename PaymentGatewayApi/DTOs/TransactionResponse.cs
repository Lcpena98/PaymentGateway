namespace PaymentGatewayApi.DTOs
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
    }
}
