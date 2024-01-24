using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ND2Assignwork.API.Models.DTO
{
    public class User_AccountDTO
    {
        public string User_Id { get; set; }

        public string User_FullName { get; set; }

        public string User_Password { get; set; }

        public string User_Phone { get; set; }

        public string User_Email { get; set; }

        public int User_Position { get; set; }

        public string User_Department { get; set; }

        public byte[] User_Image { get; set; }
        
        public bool User_IsActive { get; set; }

    }
    public class User_AccountAdminDTO
    {
        public string User_Id { get; set; }

        public string User_FullName { get; set; }

        public string User_Password { get; set; }

        public string User_Phone { get; set; }

        public string User_Email { get; set; }

        public int User_Position { get; set; }

        public string User_Department { get; set; }

    }

    public class User_AccountUserDTO
    {
        public string User_Id { get; set; }

        public string User_FullName { get; set; }

        public string User_Password { get; set; }

        public string User_Phone { get; set; }

        public string User_Email { get; set; }

        public int User_Position { get; set; }


    }
    public class PassReset
    {
        public string passwordOld { get; set; }
        public string passwordNew { get; set; }
    }
}
