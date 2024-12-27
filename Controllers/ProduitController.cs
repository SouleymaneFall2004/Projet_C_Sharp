using Microsoft.AspNetCore.Mvc;
using ExamenC_.Data;
using ExamenC_.Models;

[Route("[controller]")]
public class ProduitController : Controller
{
    private readonly AppDbContext _context;

    public ProduitController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("list")]
    public IActionResult ListProduits()
    {
        var produits = _context.Produits.ToList();
        return View(produits); 
    }

    [HttpGet("add")]
    public IActionResult AjouterProduit()
    {
        return View();
    }

    [HttpPost("add")]
    public IActionResult AjouterProduit(Produit produit)
    {
        if (ModelState.IsValid)
        {
            _context.Produits.Add(produit);
            _context.SaveChanges();
            return RedirectToAction("ListProduits");
        }
        return View(produit);
    }

    [HttpGet("edit/{id}")]
    public IActionResult ModifierStock(int id)
    {
        var produit = _context.Produits.FirstOrDefault(p => p.Id == id);
        if (produit == null)
        {
            return NotFound();
        }
        return View(produit); // Vue : Views/Produit/ModifierStock.cshtml
    }

    [HttpPost("edit/{id}")]
    public IActionResult ModifierStock(int id, int nouvelleQuantite)
    {
        var produit = _context.Produits.FirstOrDefault(p => p.Id == id);
        if (produit == null)
        {
            return NotFound();
        }

        produit.Quantite = nouvelleQuantite;
        _context.SaveChanges();
        return RedirectToAction("list");
    }
}