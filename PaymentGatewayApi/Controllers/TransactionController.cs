using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayApi.Data;
using PaymentGatewayApi.DTOs;
using PaymentGatewayApi.Services;
using PaymentGatewayApi.Models;

namespace PaymentGatewayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PaymentService _paymentService;
        public TransactionController(AppDbContext context, PaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponse>> GetTransactionById(int id)
        {
             var transaction = await _context.Transactions
                 .Include(t => t.Client)
                 .FirstOrDefaultAsync(t => t.Id == id);

             if (transaction == null)
                 return NotFound("Compra não encontrada");
            return Ok(transaction);
        }
        [HttpPost("{id}/refund")]
        public async Task<IActionResult> Refund(int id)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            var success = await _paymentService.Refund(transaction);

            if (!success)
                return BadRequest("Falha ao processar refund");

            transaction.Status = TransactionStatus.REFUNDED;

            await _context.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}