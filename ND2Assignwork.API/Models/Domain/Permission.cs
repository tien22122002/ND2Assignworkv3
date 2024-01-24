using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permission_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Permission_Name { get; set; }

        public ICollection<User_Permission> UserPermissions { get; set; }
    }
}
