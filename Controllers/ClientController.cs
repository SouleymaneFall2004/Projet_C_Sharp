using Microsoft.AspNetCore.Mvc;
using ExamenC_.Data;

[Route("[controller]")]
public class ClientController : Controller
{
    private readonly AppDbContext _context;

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("list")]
    public IActionResult ListClients()
    {
        var clients = _context.Clients.ToList();
        return View(clients);
    }
}