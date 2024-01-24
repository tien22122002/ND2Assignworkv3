using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ND2Assignwork.API.Models.DTO
{
    public class User_PermissionDTO
    {
        public string User_Id { get; set; }

        public int Permission_Id { get; set; }
    }
}
