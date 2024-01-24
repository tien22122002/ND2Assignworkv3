using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Domain
{
    public class File
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string File_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string File_Name { get; set; }

        [Required]
        [Column(TypeName = "varbinary(MAX)")]
        public byte[] FileData { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string ContentType { get; set; }

        public ICollection<Document_Send_File> Document_Send_File { get; set; }
        public ICollection<Task_File> Task_File { get; set; }
        public ICollection<Document_Incomming_File> Document_Incomming_File { get; set; }

    }
    public class FileContent
    {
        [Required] public IFormFile File { get; set; }
        public FileDTO ToFile(IFormFile file)
        {
            DateTime currentTime = DateTime.UtcNow;
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string file_id = "File" + formattedDateTime;
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            return new FileDTO
            {
                File_Id = file_id,
                File_Name = file.FileName,
                File_Data = stream.ToArray(),
                ContentType = file.ContentType
            };
        }
    }
}
