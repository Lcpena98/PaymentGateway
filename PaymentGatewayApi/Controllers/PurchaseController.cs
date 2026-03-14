using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayApi.Data;
using PaymentGatewayApi.DTOs;
using PaymentGatewayApi.Models;
using PaymentGatewayApi.Services;

namespace PaymentGatewayApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PaymentService _paymentService;

    public PurchaseController(
    AppDbContext context,
    PaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(PurchaseRequest request)
    {
        var productIds = request.Products
    .Select(p => p.ProductId)
    .ToList();

        var products = await _context.Products
    .Where(p => productIds.Contains(p.Id))
    .ToListAsync();
        if (products == null)
            return NotFound("Produto não encontrado");
        int totalAmount = 0;

        foreach (var item in request.Products)
        {
            var product = products.First(p => p.Id == item.ProductId);
            if (products.Count != request.Products.Count)
                return BadRequest("Produto inválido");
            totalAmount += product.Amount * item.Quantity;
        }
        var paymentRequest = new PaymentRequest
        {
            Amount = totalAmount,
            Name = request.Name,
            Email = request.Email,
            CardNumber = request.CardNumber,
            Cvv = request.Cvv
        };

        var result = await _paymentService.ProcessPayment(paymentRequest);
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Email == request.Email);

        if (client == null)
        {
            client = new Client
            {
                Name = request.Name,
                Email = request.Email
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        var transaction = new Transaction
        {
            ClientId = client.Id,
            Amount = totalAmount,
            Status = result.Success ? TransactionStatus.SUCCESS : TransactionStatus.FAILED,
            ExternalId = result.ExternalId,
            Gateway = result.Gateway,
            CardLastNumbers = request.CardNumber[^4..]
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        foreach (var item in request.Products)
        {
            var transactionProduct = new TransactionProduct
            {
                TransactionId = transaction.Id,
                transaction = transaction,
                ProductId = item.ProductId,

                Quantity = item.Quantity
            };
            if (transactionProduct.transaction.Status == TransactionStatus.SUCCESS)
            _context.TransactionProducts.Add(transactionProduct);
            await _context.SaveChangesAsync();
        }
        return Ok(new
        {
            message = "Compra registrada",
            amount = totalAmount
        });
    }
}