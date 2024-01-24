using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class PositionDTO
    {
        public int Position_Id { get; set; }

        public string Position_Name { get; set; }

    }
}
