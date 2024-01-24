using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;

namespace ND2Assignwork.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController, Authorize]
    public class TaskFileController : ControllerBase
    {
        private readonly ITaskFileService _taskFileService;
        public TaskFileController(ITaskFileService taskFileService)
        {
            _taskFileService = taskFileService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_taskFileService.GetAllTaskFile());
        }

        //[HttpGet("GetByTaskId/{task_id}")]
        public IActionResult GetById(string task_id)
        {
            var taskFileDTOs = _taskFileService.GetTaskFileByTaskId(task_id);
            if (taskFileDTOs == null)
            {
                return NotFound();
            }
            return Ok(taskFileDTOs);
        }
        // GET: 
        //[HttpGet("{file_id}/{task_id}")]
        public IActionResult GetById(string file_id, string task_id)
        {
            var taskFileDTO = _taskFileService.GetOneTaskFile(task_id, file_id);
            if (taskFileDTO == null)
            {
                return NotFound();
            }
            return Ok(taskFileDTO);
        }

        // POST:
        [HttpPost]
        public IActionResult CreateDocSendFile([FromBody] Task_FileDTO task_FileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_taskFileService.CreateTaskFile(task_FileDTO))
            {
                return CreatedAtAction(nameof(GetById), new { file_id = task_FileDTO.File_Id, task_id = task_FileDTO.Task_Id }, task_FileDTO);
            }
            return BadRequest("Lỗi khi thêm Task File");

        }


        // DELETE: https://localhost:7147/api/userpermissions/5
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{file_id}/{task_id}")]
        public IActionResult DeleteUserPermission([FromRoute] string file_id, string task_id)
        {
            if (_taskFileService.DeleteTaskFile(task_id, file_id))
            {
                return Ok("Đã xóa thành công !");
            }
            else { return BadRequest("Lỗi khi xóa Task File"); }

        }
    }
}
