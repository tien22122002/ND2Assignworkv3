using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Document_Send_File
    {
        [Key]
        [ForeignKey("File_Id")]
        public string File_Id { get; set; }

        [Key]
        [ForeignKey("Document_Send_Id")]
        public string Document_Send_Id { get; set; }

        [ForeignKey("User_Id")]
        public string? User_Id { get; set; }



        public File File { get; set; }
        public Document_Send Document_Send { get; set; }
        public User_Account User_Account { get; set; }
    }
}
