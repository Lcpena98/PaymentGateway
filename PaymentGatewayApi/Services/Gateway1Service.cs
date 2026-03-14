using PaymentGatewayApi.DTOs;
using PaymentGatewayApi.Gateways;
using PaymentGatewayApi.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PaymentGatewayApi.Services
{
    public class Gateway1Service : IPaymentGateway
    {
        private readonly HttpClient _httpClient;

        public Gateway1Service(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login()
        {
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:3001/login",
                new
                {
                    email = "dev@betalent.tech",
                    token = "FEC9BB078BF338F464F96B48089EB498"
                });

            var raw = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Gateway1LoginResponse>(raw);

            return result.token;
        }

        public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            var token = await Login();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var cardNumber = new string(request.CardNumber.Where(char.IsDigit).ToArray());
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:3001/transactions",
                new
                {
                    amount = request.Amount,
                    name = request.Name,
                    email = request.Email,
                    cardNumber = cardNumber,
                    cvv = request.Cvv
                });

            if (!response.IsSuccessStatusCode)
            {
                return new PaymentResult
                {
                    Success = false,
                    Gateway = "Gateway1"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<Gateway1Response>();

            return new PaymentResult
            {
                Success = true,
                ExternalId = result.id,
                Gateway = "Gateway1"
            };
        }

        public async Task<bool> Refund(string externalId)
        {
            var token = await Login();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(
                $"http://localhost:3001/transactions/:{externalId}/charge_back",
                null
            );

            return response.IsSuccessStatusCode;
        }
    }
}
