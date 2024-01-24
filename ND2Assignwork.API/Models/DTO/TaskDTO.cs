using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.DTO
{
    public class TaskDTO
    {
        public string Task_Id { get; set; }

        public string Task_Title { get; set; }

        public string Task_Content { get; set; }

        public int Task_Category { get; set; }

        public DateTime Task_DateSend { get; set; }

        public DateTime? Task_DateStart { get; set; }

        public DateTime? Task_DateEnd { get; set; }

        public string Task_Person_Send { get; set; }

        public string Task_Person_Receive { get; set; }

        public int Task_State { get; set; }
        public string Document_Send_Id { get; set; }
        public bool Task_IsSeen { get; set; }
        public DateTime? Task_TimeUpdate { get; set; }

    }
}
