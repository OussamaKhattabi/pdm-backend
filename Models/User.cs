using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace pdm.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire.")]
        public String Firstname { get; set; }


        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public String Lastname { get; set; }


        [Required(ErrorMessage = "Le mail est obligatoire.")]
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
        public String Email { get; set; }
        

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Le rôle est obligatoire")]
        [ForeignKey("RoleID")]
        public int RoleID { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }
        
        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }
      
        public ICollection<Commande>? Commandes { get; set; }
    }
}
