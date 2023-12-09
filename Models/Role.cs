using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdm.Models
{
    public class Role
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public String Name { get; set; }
        [Required(ErrorMessage = "La description est obligatoire")]
        public String Description { get; set; } = String.Empty;
    }
}
