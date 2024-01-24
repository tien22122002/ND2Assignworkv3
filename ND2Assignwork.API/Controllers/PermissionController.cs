using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;

namespace ND2Assignwork.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController, Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            this._permissionService = permissionService;
        }

        // GET: https://localhost:7147/api/permissions
        [HttpGet]
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult GetAll()
        {
            return Ok(_permissionService.GetAllPermissions());
        }

        // GET: https://localhost:7147/api/permissions/5
        //[HttpGet("{id:int}")]
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult GetById([FromRoute] int id)
        {
            var permissionDTO = _permissionService.GetPermissionById(id);
            if (permissionDTO == null)
            {
                return NotFound();
            }
            return Ok(permissionDTO);
        }

        // POST: https://localhost:7147/api/permissions
        [HttpPost]
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult CreatePermission([FromBody] PermissionDTO_Identity permissionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int id_new = _permissionService.CreatePermission(permissionDTO);
            if(id_new == 0)
            {
                return BadRequest("Lỗi khi thêm permission !");
            }
            else
            {
                var permission = _permissionService.GetPermissionById(id_new);
                return CreatedAtAction("GetById", new { id = id_new }, permission);
            }
            
        }

        // PUT: https://localhost:7147/api/permissions/5
        //[HttpPut("{id:int}")]
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult UpdatePermission([FromRoute] int id, [FromBody] PermissionDTO permissionDTO)
        {
            if (id != permissionDTO.Permission_Id)
            {
                return BadRequest("Vui nhập đúng id !");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_permissionService.UpdatePermission(permissionDTO))
            {
                return Ok(permissionDTO);
            }
            else
            {
                return BadRequest("Lỗi khi cập nhật permission !");
            }
            
        }

        // DELETE: https://localhost:7147/api/permissions/5
        //[HttpDelete("{id:int}")]
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult DeletePermission([FromRoute] int id)
        {
            if (_permissionService.DeletePermission(id))
            {
                return Ok("Đã xóa thành công Permission !");
            }
            return BadRequest("Lỗi khi xóa Permission");
        }
    }
}
