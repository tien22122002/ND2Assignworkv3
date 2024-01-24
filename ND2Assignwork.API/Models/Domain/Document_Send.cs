using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Document_Send
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Document_Send_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Document_Send_Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Document_Send_Content { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime Document_Send_Time { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Document_Send_TimeStart { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Document_Send_Deadline { get; set; }

        [Required]
        [ForeignKey("Document_Send_UserSend")]
        public string Document_Send_UserSend { get; set; }
        public User_Account UserAccount { get; set; }

        public int Document_Send_State { get; set; }
        public string? Document_Send_Comment { get; set; }
        public int Document_Send_Catagory { get; set; }
        public Task_Category Category { get; set; }
        
        public bool Document_Send_Public { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Document_Send_TimeUpdate { get; set; }





        public ICollection<User_Receive_Document> ReceivedByUsers { get; set; }
        public ICollection<Document_Send_File> Document_Send_Files { get; set; }
        public ICollection<Task> ListTask { get; set; }

    }
}
