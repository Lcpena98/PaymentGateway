using PaymentGatewayApi.Models;

namespace PaymentGatewayApi.Gateways
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessPayment(PaymentRequest request);
        Task<bool> Refund(string externalId);
    }
}
