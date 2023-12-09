using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pdm.Models
{
    public class Reservation
    {
        [Key]

        public int Id { get; set; }

        public DateTime Date { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Vehicule")]
        public int VehiculeId { get; set; }

        public Vehicule? Vehicule { get; set; }


    }
}
