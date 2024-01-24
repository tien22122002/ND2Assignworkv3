using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IUserPermissionService
    {
        Task<bool> CreateUserPermissionAsync(User_PermissionDTO userPermissionDTO);
        Task<bool> DeleteUserPermissionAsync(string user_id, int per_id);
        Task<IEnumerable<User_PermissionDTO>> GetUserPerByUserIdAsync(string user_id);

        //bool CreateUserPermission(User_PermissionDTO userPermissionDTO);


        bool DeleteUserPermission(string user_id, int per_id);
        IEnumerable<User_PermissionDTO> GetUserPermissionByUserId(string user_id);
        User_PermissionDTO GetUserPermissionById(string user_id, int per_id);
        bool isExistPer(int per_id);
        bool isExistUser(string user_id);
        IEnumerable<User_PermissionDTO> GetAllUserPermissions();
    }
}
