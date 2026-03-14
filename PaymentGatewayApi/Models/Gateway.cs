
namespace PaymentGatewayApi.Models
{
    public class Gateway
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int Priority { get; set; }
    }
}
