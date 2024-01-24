using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class Document_SendDTO
    {
        public string Document_Send_Id { get; set; }

        public string Document_Send_Title { get; set; }

        public string Document_Send_Content { get; set; }

        public DateTime Document_Send_Time { get; set; }

        public DateTime? Document_Send_TimeStart { get; set; }
        public DateTime? Document_Send_Deadline { get; set; }

        public string Document_Send_UserSend { get; set; }

        public int Document_Send_State { get; set; }
        public string? Document_Send_Comment { get; set; }
        public int Document_Send_Catagory { get; set; }
        public bool Document_Send_Public { get; set; }
        public DateTime? Document_Send_TimeUpdate { get; set; }
    }
}
