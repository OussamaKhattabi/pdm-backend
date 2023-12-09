using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDeluxeMotorSports_v1.Models
{
    public class Role
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID {  get; set; }
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public String RoleName { get; set; }
        [Required(ErrorMessage = "La description est obligatoire")]
        public String RoleDescription { get; set; } = String.Empty;
    }
}
