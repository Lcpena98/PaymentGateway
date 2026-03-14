namespace PaymentGatewayApi.DTOs
{
    public class Gateway2Request
    {
        public decimal valor { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string numeroCartao { get; set; }
        public string cvv { get; set; }
    }
}
