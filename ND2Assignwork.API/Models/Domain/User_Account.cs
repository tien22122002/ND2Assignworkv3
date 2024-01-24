using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.Domain
{
    public class User_Account
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string User_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string User_FullName { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string User_Password { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(12)")]
        public string User_Phone { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string User_Email { get; set; }


        [ForeignKey("User_Position")]
        public int User_Position { get; set; }
        public Position Position { get; set; }


        [ForeignKey("User_Department")]
        public string User_Department { get; set; }

        public Department Department { get; set; }
        public Department DepartmentOne { get; set; }

        public byte[]? User_Image { get; set; }
        
        public bool User_IsActive { get; set; }

        public ICollection<User_Receive_Document> ReceivedDocuments { get; set; }
       
        public ICollection<Task> TasksSend { get; set; }
        public ICollection<Task> TasksReceive { get; set; }
        public ICollection<Task_File> Task_File { get; set; }

        public ICollection<Document_Incomming> DocumentIncommingSend { get; set; } // Danh sách các document người dùng đã gửi
        public ICollection<Document_Incomming> DocumentIncommingReceived { get; set; }

        public ICollection<Document_Send> Document_Send { get; set; }
        public ICollection<Document_Send_File> Document_Send_File { get; set; }
        

        public ICollection<Discuss> Discusses { get; set; }

        public ICollection<User_Permission> UserPermissions { get; set; }

    }
}
