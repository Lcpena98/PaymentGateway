using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayApi.DTOs
{
    public class ProductRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}
