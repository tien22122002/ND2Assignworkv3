using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDepartmentService
    {


        bool isHead(string UserId);
        DepartmentDTO GetDepartmentById(string id);

        Task<bool> isHeadAsync(string UserId);
        //bool CreateDepartment(DepartmentDTO departmentDTO);
        //bool UpdateDepartment(DepartmentDTO departmentDTO);
        //bool UpdateDepartmentHead(string depId, string UserId);
        //ListUserByDepartmentDTO GetUserByDepartmentId(string dep_id);


        Task<DepartmentDTO> GetDepartmentByUserIdAsync(string id);
        Task<IEnumerable<DepartmentDTO>> GetAllDepartments();
        Task<IEnumerable<DepartmentDTO>> GetDepartmentByType(int type);
        Task<bool> DeleteDepartment(string id);
        Task<bool> IsHeadyAsync(string UserId);
        Task<bool> UpdateDepartmentHeadAsync(string depId, string UserId);
        Task<DepartmentDTO> GetDepartmentByIdAsync(string id);
        Task<Department> GetListUserInDepartment(string depId);
        Task<bool> UpdateDepartmentAsync(DepartmentDTO departmentDTO);
        Task<bool> CreateDepartmentAsync(DepartmentDTO departmentDTO);

    }
}
