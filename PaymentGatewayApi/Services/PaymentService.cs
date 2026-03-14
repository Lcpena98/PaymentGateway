using Microsoft.EntityFrameworkCore;
using PaymentGatewayApi.Data;
using PaymentGatewayApi.Gateways;
using PaymentGatewayApi.Models;

namespace PaymentGatewayApi.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;
        private readonly Gateway1Service _gateway1;
        private readonly Gateway2Service _gateway2;

        public PaymentService(AppDbContext context, Gateway1Service gateway1, Gateway2Service gateway2)
        {
            _context = context;
            _gateway1 = gateway1;
            _gateway2 = gateway2;
        }
        public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            var gateways = await _context.Gateways
                .Where(g => g.IsActive)
                .OrderBy(g => g.Priority)
                .ToListAsync();

            foreach (var gateway in gateways)
            {
                PaymentResult result = null;

                if (gateway.Name == "Gateway1")
                    result = await _gateway1.ProcessPayment(request);

                else if (gateway.Name == "Gateway2")
                    result = await _gateway2.ProcessPayment(request);

                if (result != null && result.Success)
                    return result;
            }

            return new PaymentResult
            {
                Success = false,
                ErrorMessage = "Todos os gateways falharam"
            };
        }
        public async Task<bool> Refund(Transaction transaction)
        {
            var gateways = await _context.Gateways
                .Where(g => g.IsActive)
                .OrderBy(g => g.Priority)
                .ToListAsync();

            foreach (var gateway in gateways)
            {
                bool result = false;

                if (gateway.Name == "Gateway1")
                    result = await _gateway1.Refund(transaction.ExternalId);

                else if (gateway.Name == "Gateway2")
                    result = await _gateway2.Refund(transaction.ExternalId);

                if (result != null && result)
                    return result;
            }

            return false;
        }
    }
}
