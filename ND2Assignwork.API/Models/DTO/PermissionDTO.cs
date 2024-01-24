using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class PermissionDTO
    {
        public int Permission_Id { get; set; }

        public string Permission_Name { get; set; }

    }
}
