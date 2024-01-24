using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;
using System.Text.RegularExpressions;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController/*,Authorize*/]
    public class TaskCategoryController : ControllerBase
    {
        private readonly ITaskCategoryService _taskCategoryService;
        public TaskCategoryController(ITaskCategoryService taskCategoryService)
        {
            _taskCategoryService = taskCategoryService;
        }
        //GET ALL TaskCategory
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_taskCategoryService.GetAllTaskCategory());
        }

        // GET: https://localhost:7147/api/taskcategory/5
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var taskCategoryDTO = _taskCategoryService.GetTaskCategoryById(id);
            if (taskCategoryDTO == null)
            {
                return NotFound();
            }
            return Ok(taskCategoryDTO);
        }

        [HttpPost]
        public IActionResult CreateTaskCategory([FromBody] TaskCategoryDTO_Identity taskCategoryDTO_Identity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*Regex regex = new Regex(@"^[a-zA-Z0-9\sÀ-ÿ]+$");

            if(!regex.IsMatch(taskCategoryDTO_Identity.Category_Name))
            {
                return BadRequest("Kí tự không hợp lệ !");
            }*/
            
            if (_taskCategoryService.CheckCategoryName(taskCategoryDTO_Identity.Category_Name))
            {
                return BadRequest("Đã tồn tại category này !");
            }
            if (_taskCategoryService.CreateTaskCategory(taskCategoryDTO_Identity))
            {
                return Ok("Thêm Catagory thành công !");
            }
            return BadRequest("Lỗi khi thêm Catagory !");
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateTaskCategory([FromRoute] int id, [FromBody] Task_CategoryDTO task_CategoryDTO)
        {
            if (id != task_CategoryDTO.Task_Category_Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _taskCategoryService.UpdateTaskCategory(task_CategoryDTO);
            return Ok(task_CategoryDTO);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTaskCategory([FromRoute] int id)
        {
            if (_taskCategoryService.DeleteTaskCategory(id))
            {
                return Ok("Xóa TaskCategory thành công !");
            }
            return BadRequest("Lỗi khi xóa taskCategory !");
        }
        
    }

}
