using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.DTOs
{
    public class PurchaseRequest
    {
        [Required]
        [MinLength(1)]
        public List<ProductRequest> Products { get; set; }
        [Required]
        [StringLength(100)]
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