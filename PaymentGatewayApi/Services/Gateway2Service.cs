using PaymentGatewayApi.DTOs;
using PaymentGatewayApi.Gateways;
using PaymentGatewayApi.Models;
using System.Text.Json;

namespace PaymentGatewayApi.Services
{
    public class Gateway2Service : IPaymentGateway
    {
        private readonly HttpClient _httpClient;
        public Gateway2Service(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Gateway-Auth-Token"))
            {
                _httpClient.DefaultRequestHeaders.Add("Gateway-Auth-Token", "tk_f2198cc671b5289fa856");
                _httpClient.DefaultRequestHeaders.Add("Gateway-Auth-Secret", "3d15e8ed6131446ea7e3456728b1211f");
            }
            var cardNumber = new string(request.CardNumber.Where(char.IsDigit).ToArray());
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:3002/transacoes",
                new
                {
                    valor = request.Amount,
                    nome = request.Name,
                    email = request.Email,
                    numeroCartao = cardNumber,
                    cvv = request.Cvv
                });

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResult
                {
                    Success = false,
                    Gateway = "Gateway2"
                };
            }
            var raw = await response.Content.ReadAsStringAsync();

            Console.WriteLine(raw);

            var result = JsonSerializer.Deserialize<Gateway2Response>(raw);

            if (result == null || string.IsNullOrEmpty(result.id))
            {
                return new PaymentResult
                {
                    Success = false,
                    Gateway = "Gateway2"
                };
            }

            return new PaymentResult
            {
                Success = true,
                ExternalId = result.id,
                Gateway = "Gateway2"
            };
        }

        public async Task<bool> Refund(string externalId)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Gateway-Auth-Token"))
            {
                _httpClient.DefaultRequestHeaders.Add("Gateway-Auth-Token", "tk_f2198cc671b5289fa856");
                _httpClient.DefaultRequestHeaders.Add("Gateway-Auth-Secret", "3d15e8ed6131446ea7e3456728b1211f");
            }
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:3002/transacoes/reembolso",
                new { id = externalId }
            );

            return response.IsSuccessStatusCode;
        }
    }
}