using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Domain;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class TaskCategoryService : ITaskCategoryService
    {

        private readonly DataContext _context;

        public TaskCategoryService(DataContext context)
        {
            this._context = context;
        }
        public IEnumerable<Task_CategoryDTO> GetAllTaskCategory()
        {
            return _context.Task_Category.Select(p => new Task_CategoryDTO
            {
                Task_Category_Id = p.Task_Category_Id,
                Category_Name = p.Category_Name,
            }).ToList();
        }
        public Task_CategoryDTO GetTaskCategoryById(int id)
        {
            var taskCategoryEntity = _context.Task_Category.Find(id);
            if (taskCategoryEntity == null)
            {
                return null;
            }

            return new Task_CategoryDTO
            {
                Task_Category_Id = taskCategoryEntity.Task_Category_Id,
                Category_Name =taskCategoryEntity.Category_Name,
            };
        }
        public bool CreateTaskCategory(TaskCategoryDTO_Identity task_CategoryDTO)
        {
            var taskCategoryEntity =new Task_Category
            {
                Category_Name = task_CategoryDTO.Category_Name,
            };
            _context.Task_Category.Add(taskCategoryEntity);
            try
            {
                int check = _context.SaveChanges();
                return check > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckCategoryName(string name)
        {

            var CategoryEntity = _context.Task_Category;
            foreach(var cate in CategoryEntity)
            {
                if (cate.Category_Name.ToLower().Equals(name.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateTaskCategory(Task_CategoryDTO task_CategoryDTO)
        {
            var taskCategoryEntity = _context.Task_Category.Find(task_CategoryDTO.Task_Category_Id);
            if(taskCategoryEntity == null) { return false; }

            taskCategoryEntity.Category_Name = task_CategoryDTO.Category_Name;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteTaskCategory(int id)
        {
            var taskCategoryEntity = _context.Task_Category.Find(id);
            if (taskCategoryEntity == null) { return false; }

            _context.Task_Category.Remove(taskCategoryEntity);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
