using Microsoft.AspNetCore.Mvc;
using PaymentGatewayApi.Data;
using PaymentGatewayApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentGatewayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClientController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            return Ok(await _context.Clients.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound("Cliente não encontrado");
            }

            return client;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            if (_context.Clients.Any(c => c.Email == client.Email))
                return BadRequest("Email já cadastrado");
            if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.Email))
                return BadRequest("Nome e email são obrigatórios");
            if (client.Transactions != null && client.Transactions.Any())
                return BadRequest("Transações não podem ser criadas junto com o cliente");
            client.Transactions = new List<Transaction>();
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllClients),new {id = client.Id});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.Id)
                return BadRequest();
            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
                return NotFound("Cliente não encontrado");
            if (_context.Clients.Any(c => c.Email == client.Email && c.Id != id))
                return BadRequest("Email já cadastrado para outro cliente");
            if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.Email))
                return BadRequest("Nome e email são obrigatórios");
            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            await _context.SaveChangesAsync();
            return NoContent();
        }
         [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound("Cliente não encontrado");
            if (_context.Transactions.Any(t => t.ClientId == id))
                return BadRequest("Não é possível excluir um cliente com transações associadas");
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

