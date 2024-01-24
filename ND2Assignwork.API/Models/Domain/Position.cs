using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Position_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Position_Name { get; set; }

        public ICollection<User_Account> Users { get; set; }
    }
}
