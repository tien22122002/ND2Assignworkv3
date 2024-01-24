using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class Document_Incomming_FileDTO
    {
        public string File_Id { get; set; }

        public string Document_Incomming_Id { get; set; }
        
    }
}
