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
    public class UserReceiveDocumentController : ControllerBase
    {
        private readonly IUserReceiveDocumentService _URDService;
        public UserReceiveDocumentController(IUserReceiveDocumentService uRDService)
        {
            _URDService = uRDService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_URDService.GetAllUserReceice());
        }

        //[HttpGet("GetByUserId/{user_id}")]
        public IActionResult GetById(string user_id)
        {
            var user_Receive_DocumentDTOs = _URDService.GetUserReceiceByUserId(user_id);
            if (user_Receive_DocumentDTOs == null)
            {
                return NotFound();
            }
            return Ok(user_Receive_DocumentDTOs);
        }
        // GET: 
        /*[HttpGet("{user_id}/{doc_id}")]
        public IActionResult GetById(string user_id, string doc_id)
        {
            var user_ReceiveDTO = _URDService.GetOneUserReceice(user_id, doc_id);
            if (user_ReceiveDTO == null)
            {
                return NotFound();
            }
            return Ok(user_ReceiveDTO);
        }*/

        // POST:
        [HttpPost]
        public IActionResult CreateUserReceive([FromBody] User_Receive_DocumentDTO userReceiveDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _URDService.CreateUserReceice(userReceiveDTO);
            return CreatedAtAction(nameof(GetById), new { user_id = userReceiveDTO.User_Id, doc_id = userReceiveDTO.Document_Send_Id }, userReceiveDTO);

        }


        // DELETE: https://localhost:7147/api/userpermissions/5
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{user_id}/{doc_id}")]
        public IActionResult DeleteUserPermission([FromRoute] string user_id, string doc_id)
        {
            if(_URDService.DeleteUserReceice(user_id, doc_id))
            {
                return Ok("Đã xóa thành công !");
            }else { return BadRequest("Lỗi khi xóa user_id"); }
            
        }
    }
}
