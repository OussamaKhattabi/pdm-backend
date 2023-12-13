using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pdm.Models
{
    public class Vehicule
    {
        [Key]
        public int VehiculeId { get; set; }
        public string Marque { get; set; }
        public string Model { get; set; }
        public int Prix { get; set; }
        public String Image { get; set; }

        [JsonIgnore]
        public ICollection<Custom> Customs { get; set; }

        [JsonIgnore]
        public ICollection<Commande> Commandes { get; set; }
        
        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; }
        
    }
}
