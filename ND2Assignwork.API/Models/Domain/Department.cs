using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Department
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Department_ID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Department_Name { get; set; }


        [ForeignKey("Department_Head")]        
        public string Department_Head { get; set; }

        public int? Department_Type { get; set; }

        public User_Account UserAccount { get; set; }

        public ICollection<User_Account> Users { get; set; }
        public ICollection<User_Receive_Document> User_Receive_Document { get; set; }
    }
}
