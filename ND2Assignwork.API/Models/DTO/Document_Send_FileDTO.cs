using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO  
{
    public class Document_Send_FileDTO
    {
        public string File_Id { get; set; }
        public string Document_Send_Id { get; set; }
        public string? User_Id { get; set; }
    }
}
