namespace ND2Assignwork.API.Models.DTO
{
    public class ListUserByDepartmentDTO
    {
        public string Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Department_Head { get; set; }
        public int? Department_Type { get; set; }
        public IEnumerable<User> Users { get; set; }

        public class User
        {
            public string User_Id { get; set; }

            public string User_FullName { get; set; }

            public string User_Email { get; set; }

            public string User_Position { get; set; }

            public string User_Department { get; set; }
            public bool User_IsActive { get; set; }
        }
    }
    
}
