using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.DTO
{
    public class Document_IncommingDTO
    {
        public string Document_Incomming_Id { get; set; }
        public string Document_Incomming_Title { get; set; }
        public string Document_Incomming_Content { get; set; }
        public DateTime Document_Incomming_Time { get; set; }
        public string Document_Incomming_UserSend { get; set; }
        public string Document_Incomming_UserReceive { get; set; }
        public int Document_Incomming_State { get; set; }
        public string? Document_Incomming_Comment { get; set; }
        public string? Document_Incomming_Id_Forward { get; set; }
        public bool Document_Incomming_IsSeen { get; set; }
        public DateTime? Document_Incomming_TimeUpdate { get; set; }

    }
    public class DocumentInDTO
    {
        public string Document_Incomming_Id { get; set; }
        public string Document_Incomming_Title { get; set; }
        public string Document_Incomming_Content { get; set; }
        public string Document_Incomming_Time { get; set; }
        public string Document_Incomming_UserSend { get; set; }
        public string Deparment_NameReceive { get; set; }
        public string Document_Incomming_UserReceive { get; set; }
        public int Document_Incomming_State { get; set; }
        public string Document_Incomming_Comment { get; set; }
        public string Document_Incomming_Id_Forward { get; set; }
        public string Department_Location { get; set; }
    }
}
