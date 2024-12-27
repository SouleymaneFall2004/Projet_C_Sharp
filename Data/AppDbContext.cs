using Microsoft.EntityFrameworkCore;
using ExamenC_.Models;

namespace ExamenC_.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<CommandeProduit> CommandeProduits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CommandeProduit>()
                .HasKey(cp => new { cp.CommandeId, cp.ProduitId });

            modelBuilder.Entity<CommandeProduit>()
                .HasOne(cp => cp.Commande)
                .WithMany(c => c.CommandeProduits)
                .HasForeignKey(cp => cp.CommandeId);

            modelBuilder.Entity<CommandeProduit>()
                .HasOne(cp => cp.Produit)
                .WithMany(p => p.CommandeProduits)
                .HasForeignKey(cp => cp.ProduitId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Commandes)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Commande>()
                .HasMany(c => c.Paiements)
                .WithOne(p => p.Commande)
                .HasForeignKey(p => p.CommandeId);
        }
    }
}