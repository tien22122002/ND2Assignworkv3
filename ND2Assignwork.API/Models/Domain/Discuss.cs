using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.Domain
{
    public class Discuss
    {
        [Key]
        public string Discuss_Task { get; set; }

        [Key]
        public string Discuss_User { get; set; }

        [Key]
        [Column(TypeName = "datetime")]
        public DateTime Discuss_Time { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Discuss_Content { get; set; }
        public bool Discuss_IsSeen { get; set; }
        public Task Task { get; set; }
        public User_Account UserAccount { get; set; }
    }

}
