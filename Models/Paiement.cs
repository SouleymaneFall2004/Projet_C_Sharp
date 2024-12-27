namespace ExamenC_.Models {
    public class Paiement
    {
        public int Id { get; set; }
        public string? Reference { get; set; }
        public double Montant { get; set; }
        public TypePaiement Type { get; set; }

        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
    }
}