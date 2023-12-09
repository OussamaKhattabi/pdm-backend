using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PremiumDeluxeMotorSports_v1.Models
{
    public class Custom
    {
        [Key]
        public int CustomId { get; set; }
        public string Couleur { get; set; }
        public int Stage {  get; set; }
        public double PrixCstm { get; set; }
        
        [ForeignKey("Vehicule")]
        public int VehiculeId { get; set; }
        [JsonIgnore]
        public Vehicule? Vehicule { get; set; }
        
        [JsonIgnore]
        public ICollection<Commande>? Commandes { get; set; }

        public Custom()
        {
        }

        public Custom(int customId, string couleur, int stage, double prixCstm, int vehiculeId)
        {
            CustomId = customId;
            Couleur = couleur;
            Stage = stage;
            PrixCstm = prixCstm;
            VehiculeId = vehiculeId;


        }
    }
}
