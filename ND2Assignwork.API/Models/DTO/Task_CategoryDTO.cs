using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.DTO
{
    public class Task_CategoryDTO
    {
        public int Task_Category_Id { get; set; }

        public string Category_Name { get; set; }

    }
}
