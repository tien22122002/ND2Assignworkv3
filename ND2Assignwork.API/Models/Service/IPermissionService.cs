using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;

namespace ND2Assignwork.API.Models.Service
{
    public interface IPermissionService
    {
        IEnumerable<PermissionDTO> GetAllPermissions();
        PermissionDTO GetPermissionById(int id);
        int CreatePermission(PermissionDTO_Identity permissionDTO);
        bool UpdatePermission(PermissionDTO permissionDTO);
        bool DeletePermission(int id);
    }
}
