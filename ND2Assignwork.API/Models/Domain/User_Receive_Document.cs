using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class User_Receive_Document
    {
        [Key]
        [ForeignKey("User_Id")]
        public string User_Id { get; set; }

        [Key]
        [ForeignKey("Document_Send_Id")]
        public string Document_Send_Id { get; set; }

        [ForeignKey("Department_Id")]
        public string? Department_Id { get; set; }

        public bool Document_Send_IsSeen { get; set; }

        public User_Account User_Account { get; set; }
        public Document_Send Document_Send { get; set; }
        public Department Department { get; set; }


    }
}
