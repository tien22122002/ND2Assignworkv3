using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;

namespace ND2Assignwork.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController, Authorize]
    public class DiscussController : ControllerBase
    {
        private readonly IDiscussService _discussService;
        public DiscussController(IDiscussService discussService)
        {
            _discussService = discussService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_discussService.GetAll());
        }

        //[HttpGet("GetByTaskId/{TaskId}")]
       /* public IActionResult GetByTaskId([FromRoute] string TaskId)
        {
            var discussDTO = _discussService.GetByTaskId(TaskId);
            if (discussDTO == null)
            {
                return NotFound();
            }
            return Ok(discussDTO);
        }*/

        [HttpPost]
        public IActionResult CreateDiscuss([FromBody] DiscussDTO discussDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_discussService.Create(discussDTO))
            {
                return CreatedAtAction("GetByTaskId", new { TaskId = discussDTO.Discuss_Task }, discussDTO);
            }
            else
            {
                return BadRequest("Lỗi khi thêm Discuss !");
            }
        }

        
        //[HttpDelete("{TaskId}/{UserId}/{time}")]
        public IActionResult DeleteDiscuss([FromRoute] string TaskId, string UserId, DateTime time)
        {
            var discussDTO = _discussService.GetDiscuss(TaskId, UserId, time);
            if (discussDTO == null)
            {
                return BadRequest("Discuss không tồn tại !");
            }
            if (_discussService.DeleteDiscuss(TaskId, UserId, time))
            {
                return BadRequest("Đã xóa Discuss thành công !");
            }
            else
            {
                return BadRequest("Lỗi khi xóa Discuss !");
            }
        }
    }
}
