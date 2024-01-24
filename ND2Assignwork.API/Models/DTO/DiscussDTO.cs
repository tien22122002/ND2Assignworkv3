using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class DiscussDTO
    {
        public string Discuss_Task { get; set; }

        public string Discuss_User { get; set; }

        public DateTime Discuss_Time { get; set; }
        public string Discuss_Content { get; set; }
        public bool Discuss_IsSeen { get; set; }
    }
}
