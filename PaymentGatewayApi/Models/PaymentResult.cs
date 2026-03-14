namespace PaymentGatewayApi.Models
{
    public class PaymentResult
    {
        public bool Success { get; set; }

        public string ExternalId { get; set; }

        public string ErrorMessage { get; set; }

        public string Gateway { get; set; }
    }
}
