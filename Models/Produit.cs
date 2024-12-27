namespace ExamenC_.Models {
public class Produit
    {
        public int Id { get; set; }
        public required string Libelle { get; set; }
        public int Quantite { get; set; }
        public double PrixUnitaire { get; set; }
        public int? QuantiteSeuil { get; set; }
        public string? Images { get; set; }

        public ICollection<CommandeProduit>? CommandeProduits { get; set; }
    }
}