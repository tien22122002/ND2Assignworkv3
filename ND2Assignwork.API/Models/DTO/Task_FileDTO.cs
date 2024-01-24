using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.DTO
{
    public class Task_FileDTO
    {
        public string File_Id { get; set; }


        public string Task_Id { get; set; }

        public string User_Id { get; set; }
        
    }
}
