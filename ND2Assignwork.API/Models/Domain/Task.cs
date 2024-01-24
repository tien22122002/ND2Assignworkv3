using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.Domain
{
    public class Task
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Task_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Task_Title { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Task_Content { get; set; }


        [ForeignKey("Task_Category")]
        public int Task_Category { get; set; }
        public Task_Category Category { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime Task_DateSend { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Task_TimeStart { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Task_DateEnd { get; set; }

        [Required]
        [ForeignKey("Task_Person_Send")]
        public string Task_Person_Send { get; set; }
        public User_Account Sender { get; set; }

        [Required]
        [ForeignKey("Task_Person_Send")]
        public string Task_Person_Receive { get; set; }
        public User_Account Receiver { get; set; }


        [Required]
        [ForeignKey("Document_Send_Id")]
        public string Document_Send_Id { get; set; }
        public Document_Send Document_Send { get; set; }



        public int Task_State { get; set; }
        public bool Task_IsSeen { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Task_TimeUpdate { get; set; }

       

        public ICollection<Discuss> Discusses { get; set; }
        public ICollection<Task_File> TaskFile { get; set; }

    }
}
