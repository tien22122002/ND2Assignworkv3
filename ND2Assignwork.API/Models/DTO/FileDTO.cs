using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class FileDTO
    {
        public string File_Id { get; set; }

        public string File_Name { get; set; }

        public byte[] File_Data { get; set; }
        public string ContentType { get; set; }
    }
}
