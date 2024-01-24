using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.Domain
{
    public class Document_Incomming
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Document_Incomming_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Document_Incomming_Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Document_Incomming_Content { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime Document_Incomming_Time { get; set; }

        [Required]
        [ForeignKey("Document_Incomming_UserSend")]        
        public string Document_Incomming_UserSend { get; set; }
        public User_Account Sender { get; set; }

        [Required]
        [ForeignKey("Document_Incomming_UserReceive")]
        public string Document_Incomming_UserReceive { get; set; }
        public User_Account Receiver { get; set; }

        public int Document_Incomming_State { get; set; }
        public string? Document_Incomming_Comment { get; set; }

        [ForeignKey("Document_Incomming_Id_Forward")]
        public string Document_Incomming_Id_Forward { get; set; }

        public bool Document_Incomming_IsSeen { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Document_Incomming_TimeUpdate { get; set; }

        public Document_Incomming ForwardDocument { get; set; }
        public ICollection<Document_Incomming_File> Document_Incomming_File { get; set; }
        
    }
}
