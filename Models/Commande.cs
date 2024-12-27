namespace ExamenC_.Models {
    public class Commande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public double Montant { get; set; }
        public Etat Etat { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public required ICollection<CommandeProduit> CommandeProduits { get; set; }
        public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();
    }
}