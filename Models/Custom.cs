using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pdm.Models
{
    public class Custom
    {
        [Key]
        public int Id { get; set; }
        public string Couleur { get; set; }
        public int Stage {  get; set; }
        public double PrixCustom { get; set; }
        
        [ForeignKey("Vehicule")]
        public int VehiculeId { get; set; }
        [JsonIgnore]
        public Vehicule? Vehicule { get; set; }
        
        [JsonIgnore]
        public ICollection<Commande>? Commandes { get; set; }
    }
}
