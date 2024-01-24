using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class DepartmentDTO
    {
        
        public string Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Department_Head { get; set; }
        public int? Department_Type { get; set; }
    }
}
