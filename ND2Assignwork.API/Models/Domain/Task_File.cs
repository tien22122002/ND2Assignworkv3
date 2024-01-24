using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.Domain
{
    public class Task_File
    {
        [Key]
        [ForeignKey("File_Id")]
        public string File_Id { get; set; }
        public File File { get; set; }


        [Key]
        [ForeignKey("Task_Id")]
        public string Task_Id { get; set; }
        public Task Task { get; set; }

        [ForeignKey("User_Id")]
        public string? User_Id { get; set; }
        public User_Account Account { get; set; }

    }
}
