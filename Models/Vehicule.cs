using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PremiumDeluxeMotorSports_v1.Models
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


        [ForeignKey("ReservationId")]
        [JsonIgnore]
        public int? ReservationId { get; set; }

        [JsonIgnore]
        public Reservation? Reservation { get; set; }

        public Vehicule()
        {
            Customs = new List<Custom>();
            Commandes = new List<Commande>();

        }

        public Vehicule(int id,String marque,string model, int prix, string image)
        {
            VehiculeId = id;
            Marque = marque;
            Model = model;
            Prix = prix;
            Image = image;

            Customs = new List<Custom>();
            Commandes = new List<Commande>();

        }
    }
}
