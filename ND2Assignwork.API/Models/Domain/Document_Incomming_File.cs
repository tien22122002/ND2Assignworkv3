using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Document_Incomming_File
    {
        [Key]
        [ForeignKey("File_Id")]
        public string File_Id { get; set; }

        [Key]
        [ForeignKey("Document_Incomming_Id")]
        public string Document_Incomming_Id { get; set; }

        public File File { get; set; }
        public Document_Incomming Document_Incomming { get; set; }
    }
}
