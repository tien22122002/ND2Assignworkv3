using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using static ND2Assignwork.API.Models.DTO.ListUserByDepartmentDTO;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IUserAccountService _userAccountService;
        private readonly IPositionService _positionService;

        public DepartmentController(IDepartmentService departmentService, IUserAccountService userAccountService, IPositionService positionService)
        {
            this._departmentService = departmentService;
            this._userAccountService = userAccountService;
            _positionService = positionService;
        }

        // GET: https://localhost:7147/api/departments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllDepartments());
        }

        // GET: https://localhost:7147/api/departments/5
        [HttpGet("GetByDepartmentId/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var departmentDTO = await _departmentService.GetDepartmentByIdAsync(id);
            if (departmentDTO == null)
            {
                return BadRequest("Tìm thấy department !");
            }
            return Ok(departmentDTO);
        }
        [HttpGet("GetUserInDepartment"), Authorize(Roles ="Admin, SuperAdmin")]
        public async Task<IActionResult> GetUserByDepartment()
        {
            var user =await _userAccountService.GetUserAccountByIdAsync(User.Identity.Name);
            
            var departmentById =await _departmentService.GetListUserInDepartment(user.Department.Department_ID);
            if (departmentById == null)
            {
                return BadRequest("Tìm thấy department nào !");
            }

            return Ok(new
            {
                departmentById.Department_ID,
                departmentById.Department_Head,
                departmentById.Department_Name,
                departmentById.Department_Type,
                Users = departmentById.Users.Select(u => new
                {
                    u.User_Id,
                    u.User_FullName,
                    u.User_Email,
                    u.User_Phone,
                    u.Position.Position_Name,
                    u.User_IsActive
                }).ToList()
            });
        }

        [HttpGet("GetUserByDepartmentId/{id}")]
        public async Task<IActionResult> GetUserByDepartmentID(string id)
        {
            var departmentById = await _departmentService.GetListUserInDepartment(id);
            if (departmentById == null)
            {
                return BadRequest("Không có department nào !");
            }
            return Ok(new
            {
                departmentById.Department_ID,
                departmentById.Department_Head,
                departmentById.Department_Name,
                departmentById.Department_Type,
                Users = departmentById.Users.Select(u => new
                {
                    u.User_Id,
                    u.User_FullName,
                    u.User_Email,
                    u.User_Phone,
                    u.Position.Position_Name,
                    u.User_IsActive
                }).ToList()
            });
        }
        // POST: https://localhost:7147/api/departments
        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            if (await _departmentService.CreateDepartmentAsync(departmentDTO))
            {
                return Ok("Thêm mới thành công.");
            }
            return BadRequest("Lỗi khi thêm department !");
        }
        [HttpGet("GetDepartmentByType/{type:int}")]
        public async Task<IActionResult> GetDepartmentByType(int type)
        {
            var listDepartmentDTO = await _departmentService.GetDepartmentByType(type);
            if (listDepartmentDTO == null)
            {
                return NotFound();
            }
            return Ok(listDepartmentDTO);
        }
        [HttpPut("UpdateActiveUser"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateActiveUser(string userId)
        {
            if (userId.Equals(User.Identity.Name))
                return BadRequest("Không tự thay đổi trạng thái của mình được ");
            if(await _userAccountService.UpdateActiveUserAccount(userId))
                return Ok("Thay đổi trạng thái thành công.");
            return BadRequest("Lỗi khi thay đổi trạng thái");
        }
        [HttpPut("UpdatePositionUser"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePositionUser(string userId, int position)
        {
            if(await _positionService.GetPositionByIdAsnyc(position) == null)
            {
                return BadRequest("Chức vụ không tồn tại !");
            }
            if (userId.Equals(User.Identity.Name))
                return BadRequest("Không tự thay đổi chức vụ của mình được");
            if (await _userAccountService.UpdatePsitionUserAccount(userId, position))
                return Ok("Thay đổi chức vụ thành công.");
            return BadRequest("Lỗi khi thay đổi chức vụ");
        }

        // PUT: https://localhost:7147/api/departments/5
        [HttpPut("{id}"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] string id, [FromBody] DepartmentDTO departmentDTO)
        {
            if (id != departmentDTO.Department_ID)
            {
                return BadRequest("Vui lòng nhập đúng id !");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _departmentService.UpdateDepartmentAsync(departmentDTO))
            {
                return Ok(departmentDTO);
            }
            else
            {
                return BadRequest("Cập nhật department không thành công !");
            }
        }

        // DELETE: https://localhost:7147/api/departments/5
        [HttpDelete("{id}"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] string id)
        {
            if (await _departmentService.DeleteDepartment(id))
            {
                return BadRequest("Xóa department thành công !");
            }
            return BadRequest("Lỗi khi xóa department !");
        }

    }
}
