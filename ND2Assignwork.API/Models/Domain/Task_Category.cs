using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.Domain
{
    public class Task_Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Task_Category_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Category_Name { get; set; }

        public ICollection<Task> LstTaskCategory { get; set; }

        public ICollection<Document_Send> Document_Send { get; set; }
    }
}
