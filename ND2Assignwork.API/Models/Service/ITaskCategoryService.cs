using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface ITaskCategoryService
    {
        IEnumerable<Task_CategoryDTO> GetAllTaskCategory();
        Task_CategoryDTO GetTaskCategoryById(int id);
        bool CreateTaskCategory(TaskCategoryDTO_Identity task_CategoryDTO);
        bool UpdateTaskCategory(Task_CategoryDTO task_CategoryDTO);
        bool DeleteTaskCategory(int id);
        bool CheckCategoryName(string name);


    }
}
