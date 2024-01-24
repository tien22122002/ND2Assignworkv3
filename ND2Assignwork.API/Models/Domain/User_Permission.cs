using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class User_Permission
    {
        [Key]
        [ForeignKey("User_Id")]
        public string User_Id { get; set; }
        public User_Account UserAccount { get; set; }


        [Key]
        [ForeignKey("Permission_Id")]
        public int Permission_Id { get; set; }
        public Permission Permission { get; set; }
    }
}
