using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class User_Receive_DocumentDTO
    {
        public string User_Id { get; set; }

        public string Document_Send_Id { get; set; }
        public string Department_Id { get; set; }
        public bool Document_Send_IsSeen { get; set; }
    }
}
