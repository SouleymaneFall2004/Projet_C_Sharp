using Microsoft.AspNetCore.Mvc;
using ExamenC_.Data;
using ExamenC_.Models;
using Microsoft.EntityFrameworkCore;

[Route("[controller]")]
public class CommandeController : Controller
{
    private readonly AppDbContext _context;

    public CommandeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("list")]
    public IActionResult ListCommandes()
    {
        var commandes = _context.Commandes.Where(c => c.Etat == Etat.EN_ATTENTE).ToList();
        return View(commandes);
    }

    [HttpGet("filtrer")]
    public IActionResult ListCommandes(Etat etat)
    {
        var commandes = _context.Commandes
            .Where(c => c.Etat == etat)
            .ToList();

        return View(commandes);
    }

    [HttpGet("listByClient")]
    public IActionResult ListByClient(int clientId)
    {
        var client = _context.Clients.Find(clientId);
        var commandes = _context.Commandes.Where(c => c.ClientId == clientId).ToList();
        if (client != null)
        {
            ViewData["ClientName"] = $"{client.Prenom} {client.Nom}";
            ViewData["ClientId"] = clientId;
        }
        return View("ListCommandes", commandes);
    }

    [HttpPost("changeStateToReceived/{commandeId}")]
    public IActionResult ChangeStateToReceived(int commandeId)
    {
        var commande = _context.Commandes.Find(commandeId);

        if (commande == null)
        {
            return NotFound("Commande non trouvée");
        }

        if (commande.Etat == Etat.A_LIVRER)
        {
            commande.Etat = Etat.RECU;
            _context.SaveChanges();
        }
        return RedirectToAction("ListByClient", new { clientId = commande.ClientId });
    }

    [HttpPost("changeStateToDeliver/{commandeId}")]
    public IActionResult ChangeStateToDeliver(int commandeId)
    {
        var commande = _context.Commandes
            .Include(c => c.CommandeProduits)
            .ThenInclude(cp => cp.Produit)
            .FirstOrDefault(c => c.Id == commandeId);

        if (commande == null)
        {
            return NotFound("Commande non trouvée");
        }

        foreach (var commandeProduit in commande.CommandeProduits)
        {
            var produit = commandeProduit.Produit;
            if (produit != null)
            {
                if (produit.Quantite >= commandeProduit.Quantite)
                {
                    produit.Quantite -= commandeProduit.Quantite;
                }
                else
                {
                    return BadRequest($"Stock insuffisant pour le produit : {produit.Libelle}");
                }
            }
        }

        if (commande.Etat == Etat.EN_ATTENTE)
        {
            commande.Etat = Etat.A_LIVRER;
            _context.SaveChanges();
        }

        return RedirectToAction("list");
    }

    [HttpGet("create/{clientId}")]
    public IActionResult CreerCommande(int clientId)
    {
        ViewBag.ClientId = clientId;
        ViewBag.Produits = _context.Produits.ToList();
        return View();
    }

    [HttpPost("create")]
    public IActionResult CreerCommande(int clientId, List<int> produitIds, List<int> quantites)
    {
        if (produitIds == null || quantites == null || produitIds.Count != quantites.Count)
        {
            ModelState.AddModelError("", "Données invalides pour les produits.");
            ViewBag.Produits = _context.Produits.ToList();
            return View();
        }

        double montantTotal = 0;
        var commandeProduit = new List<CommandeProduit>();

        for (int i = 0; i < produitIds.Count; i++)
        {
            var produit = _context.Produits.FirstOrDefault(p => p.Id == produitIds[i]);
            if (produit == null) continue;

            montantTotal += produit.PrixUnitaire * quantites[i];

            commandeProduit.Add(new CommandeProduit
            {
                ProduitId = produit.Id,
                Quantite = quantites[i]
            });
        }

        var commande = new Commande
        {
            ClientId = clientId,
            CommandeProduits = commandeProduit,
            Etat = Etat.EN_ATTENTE,
            Montant = montantTotal,
        };

        _context.Commandes.Add(commande);
        _context.SaveChanges();

        return RedirectToAction("ListByClient", new { clientId = commande.ClientId });
    }
}