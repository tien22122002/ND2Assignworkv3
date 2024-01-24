using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _userPermissionService;

        public UserPermissionController(IUserPermissionService userPermissionService)
        {
            this._userPermissionService = userPermissionService;
        }

        // GET: https://localhost:7147/api/userpermissions
        
        /*[HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userPermissionService.GetAllUserPermissions());
        }*/

        [HttpGet("GetByUserId/{user_id}")]
        public async Task<IActionResult> GetById(string user_id)
        {
            var userPermissionDTO = await _userPermissionService.GetUserPerByUserIdAsync(user_id);
            if (userPermissionDTO == null)
            {
                return BadRequest("Không tìm thấy quyền của User.");
            }
            return Ok(userPermissionDTO);
        }


        [HttpGet("GetByUserLogin")]
        public async Task<IActionResult> GetByLoggedInUser()
        {
            var userPermissionDTO =await _userPermissionService.GetUserPerByUserIdAsync(User.Identity.Name);
            if (userPermissionDTO == null)
            {
                return Ok("User không có quyền.");
            }
            return Ok(userPermissionDTO);
        }

        [Authorize(Roles = "SupperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserPermission([FromBody] User_PermissionDTO userPermissionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _userPermissionService.CreateUserPermissionAsync(userPermissionDTO))
                return BadRequest("Đã thêm quyền thành công !");
            return BadRequest("Lỗi khi thêm UserPermission.");
        }


        // DELETE: https://localhost:7147/api/userpermissions/5
        [Authorize(Roles = "SupperAdmin")]
        [HttpDelete("{user_id}/{per_id:int}")]
        public async Task<IActionResult> DeleteUserPermission([FromRoute] string user_id, int per_id)
        {
            if (await _userPermissionService.DeleteUserPermissionAsync(user_id, per_id))
                return Ok("Xóa UserPermission thành công.");
            return BadRequest("Lỗi khi xóa userPermission !");
        }
    }
}
