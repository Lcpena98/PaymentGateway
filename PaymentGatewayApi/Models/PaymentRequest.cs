using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.Models
{
    public class PaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Range(1,999)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Cvv { get; set; }
    }
}
