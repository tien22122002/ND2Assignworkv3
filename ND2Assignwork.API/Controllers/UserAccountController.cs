using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ND2Assignwork.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IConfiguration _configuration;
        private readonly IUserPermissionService _userPermissionService;
        private readonly IPermissionService _permissionService;
        private readonly IDepartmentService _departmentService;
        private readonly IDocumentIncommingService _documentIncommingService;
        private readonly IDocumentSendService _documentSendService;
        private readonly ITaskService _taskService;
        private readonly IDiscussService _discussService;


        public UserAccountController(
            IUserAccountService userAccountService,
            IConfiguration configuration,
            IUserPermissionService userPermissionService,
            IPermissionService permissionService,
            IDepartmentService departmentService,
            IDocumentIncommingService documentIncommingService,
            IDocumentSendService documentSendService,
            ITaskService taskService,
            IDiscussService discussService
            )
        {
            _userAccountService = userAccountService;
            _configuration = configuration;
            _userPermissionService = userPermissionService;
            _permissionService = permissionService;
            _departmentService = departmentService;
            _documentSendService = documentSendService;
            _documentIncommingService = documentIncommingService;
            _taskService = taskService;
            _discussService = discussService;
        }
        

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // Kiểm tra xem người dùng đã nhập đúng tên người dùng và mật khẩu chưa
            var userAccount = await _userAccountService.GetUserAccountByIdAsync(loginDTO.UserId);
            if (userAccount == null)
                return BadRequest("Tài khoản hoặc mật khẩu không chính xác !");
            if (!userAccount.User_IsActive)
                return BadRequest("Tài khoản không còn hoạt động, hãy liêu hệ với trưởng phòng hoặc Super Admin để được giải quyết !");
            // Kiểm tra xem mật khẩu đã nhập có đúng với mật khẩu đã lưu trong cơ sở dữ liệu hay không
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, userAccount.User_Password))
                return BadRequest("Tài khoản hoặc mật khẩu không chính xác !");
            /*var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true,
                Secure = false,
            };

            Response.Cookies.Append("auth", userAccount.User_Id, cookieOptions);*/


            var tokenDTO = new TokenDTO { Token =await CreateToken(userAccount) };
           
            
            
            var response = new
            {
                TokenDTO = tokenDTO,
                Roles = userAccount.UserPermissions?.Select(u => u.Permission.Permission_Name).ToList(),
                UserId = userAccount.User_Id,
                UserFullName = userAccount.User_FullName,
                UserEmail = userAccount.User_Email,
                Department = new
                {
                    userAccount.Department?.Department_ID,
                    userAccount.Department?.Department_Name,
                    userAccount.Department?.Department_Head,
                    userAccount.Department?.Department_Type
                }
            };


            return Ok(response);

        }

        // POST: https://localhost:7147/api/useraccounts
        [HttpPost("RegisterUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserAccount([FromBody] User_AccountUserDTO userAccountAdminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserLogin =await _userAccountService.GetUserAccountByIdAsync(User.Identity.Name);
            if (UserLogin == null)
            { return BadRequest("Không tìm thấy thông tin user đang login !"); }
            if (UserLogin.Department.Department_Head != UserLogin.User_Id)
                return BadRequest("Không phải là trưởng phòng không sử dụng được api này !");
            if (await _userAccountService.GetUserAccountByIdAsync(userAccountAdminDTO.User_Id) != null)
            {
                return BadRequest("Tài khoản đã tồn tại !");
            }
            var userAccountDTO = new User_AccountDTO
            {
                User_Id = userAccountAdminDTO.User_Id,
                User_FullName = userAccountAdminDTO.User_FullName,
                User_Password = userAccountAdminDTO.User_Password,
                User_Phone = userAccountAdminDTO.User_Phone,
                User_Email = userAccountAdminDTO.User_Email,
                User_Position = userAccountAdminDTO.User_Position,
                User_Department = UserLogin.User_Department,
                User_Image = null,
                User_IsActive = true
            };
            if (await _userAccountService.CreateUserAccount(userAccountDTO))
            {
                var permissionDTO = new User_PermissionDTO
                {
                    User_Id = userAccountDTO.User_Id,
                    Permission_Id = 3
                };
                if(await _userPermissionService.CreateUserPermissionAsync(permissionDTO))
                    return Ok("Đã tạo tài khoản thành công !");
                return BadRequest("Tạo tài khoản thành công nhưng lỗi khi gắn quyền.");
            }
            else
            {
                return BadRequest("Lỗi khi tạo tài khoản.");
            }
        }
        [HttpPost("RegisterAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateUserAccountAdmin([FromBody] User_AccountAdminDTO userAccountAdminDTO)
        {
            var dep =await _departmentService.GetDepartmentByIdAsync(userAccountAdminDTO.User_Department);
            if (dep == null) return BadRequest("Department không tồn tại !");
            if (dep.Department_Head != null) return BadRequest("Department " + dep.Department_Name + " đã có Department Head !");
            if (await _userAccountService.GetUserAccountByIdAsync(userAccountAdminDTO.User_Id) != null)
            {
                return BadRequest("Tài khoản đã tồn tại !");
            }
            var userAccountDTO = new User_AccountDTO
            {
                User_Id = userAccountAdminDTO.User_Id,
                User_FullName = userAccountAdminDTO.User_FullName,
                User_Password = userAccountAdminDTO.User_Password,
                User_Phone = userAccountAdminDTO.User_Phone,
                User_Email = userAccountAdminDTO.User_Email,
                User_Position = userAccountAdminDTO.User_Position,
                User_Department = userAccountAdminDTO.User_Department,
                User_Image = null,
                User_IsActive = true
            };
            if (await _userAccountService.CreateUserAccount(userAccountDTO))
            {
                if (await _departmentService.UpdateDepartmentHeadAsync(dep.Department_ID, userAccountAdminDTO.User_Id))
                    return BadRequest("Lỗi khi gắn vị trí trưởng phòng. ");
                var permissionDTO = new User_PermissionDTO
                {
                    User_Id = userAccountDTO.User_Id,
                    Permission_Id = 3
                };
               
                var permission2DTO = new User_PermissionDTO
                {
                    User_Id = userAccountDTO.User_Id,
                    Permission_Id = 2
                };
                if(await _userPermissionService.CreateUserPermissionAsync(permission2DTO) && await _userPermissionService.CreateUserPermissionAsync(permissionDTO))
                    return Ok("Đã tạo tài khoản thành công !");
                return BadRequest("Lỗi khi gắn quyền cho tài khoản. Tài khoản đã được tại");
            }
            else
            {
                return BadRequest("Lỗi khi tạo tài khoản.");
            }
        }

        /*[HttpGet("GetUserLogin"), Authorize]
        public IActionResult GetUser()
        {
            string userId = User.Identity.Name;

            var userAccountDTO = _userAccountService.GetUserAccountById(userId);
            if (userAccountDTO == null)
            {
                return Ok(userId);
            }
            var userPermissionDTO = _userPermissionService.GetUserPermissionByUserId(userId);
            var roles = new List<string>();
            if (userPermissionDTO != null)
            {
                foreach (User_PermissionDTO user_PermissionDTO in userPermissionDTO)
                {
                    var permission = _permissionService.GetPermissionById(user_PermissionDTO.Permission_Id);
                    if (permission != null)
                    {
                        string permissionName = permission.Permission_Name;
                        roles.Add(permissionName);
                    }
                }
            }
            var response = new
            {
                Roles = roles,
                UserId = userAccountDTO.User_Id,
                UserFullName = userAccountDTO.User_FullName,
                UserEmail = userAccountDTO.User_Email,
                Department = _departmentService.GetDepartmentById(userAccountDTO.User_Department),
            };
            return Ok(response);
        }*/
        [HttpGet("GetUserLogin"), Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                var userAccountDTO = await _userAccountService.GetUserAccountByIdAsync(User.Identity.Name);

                if (userAccountDTO == null)
                {
                    return NotFound();
                }

                // Xử lý dữ liệu và trả về
                return Ok(new
                {
                    Roles = userAccountDTO.UserPermissions?.Select(u => u.Permission.Permission_Name).ToList(),
                    UserId = userAccountDTO.User_Id,
                    UserFullName = userAccountDTO.User_FullName,
                    UserEmail = userAccountDTO.User_Email,
                    Department = new
                    {
                        userAccountDTO.Department?.Department_ID,
                        userAccountDTO.Department?.Department_Name,
                        userAccountDTO.Department?.Department_Head,
                        userAccountDTO.Department?.Department_Type
                    }
                });
            }
            catch (Exception ex)
            {
                // Xử lý exception và ghi log nếu cần
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("GetTotalNumberNotification"), Authorize]
        public async Task<IActionResult> GetTotalNumberNotification()
        {
            var userId = User.Identity.Name;
            var numberDocIn =await _documentIncommingService.GetListByUserReceiveIsSeen(userId);
            int numberDocSend = _documentSendService.GetNumberNotification(userId);
            int numberTask = _taskService.getNumberNotification(userId);
            int numberDiscuss = _discussService.GetNumberNitification(userId);
            int totalNumber = numberDocIn.Count() + numberDocSend + numberTask + numberDiscuss;
            return Ok(new
            {
                totalNumber,
                numberDocIn = numberDocIn.Count(),
                numberDocSend,
                numberTask,
                numberDiscuss
            });
        }


        // GET: https://localhost:7147/api/useraccounts
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userAccountService.GetAllUserAccounts());
        }

        // GET: https://localhost:7147/api/useraccounts/5
        [HttpGet("GetByUserId/{id}"), Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var userAccountDTO =await _userAccountService.GetUserAccountByIdAsync(id);
            if (userAccountDTO == null)
            {
                return BadRequest("Tài khoản không tồn tại !");
            }
            return Ok(new
            {
                userAccountDTO.User_Id,
                userAccountDTO.User_FullName,
                userAccountDTO.User_Email,
                userAccountDTO.User_Phone,
                userAccountDTO.Department?.Department_Name,
                userAccountDTO.Position?.Position_Name,

            });
        }


        // PUT: https://localhost:7147/api/useraccounts/5
        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAccount([FromBody] User_AccountDTO userAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userAccountDTO.User_Id = User.Identity.Name;
            if (await _userAccountService.UpdateUserAccount(userAccountDTO))
            {
                return Ok(userAccountDTO);
            }
            else
            {
                return BadRequest("Lỗi khi cập nhật thông tin !");
            }
        }
        
        // PUT: https://localhost:7147/api/useraccounts/5
        [HttpPut("ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] PassReset passReset )
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string userId = User.Identity.Name;
            var userAccountEntity = await _userAccountService.GetUserAccountByIdAsync(userId);
            /*if (userAccountEntity == null)
            {
                return BadRequest("Tài khoản không tồn tại !");
            }*/
            
            if (await _userAccountService.ResetPassword(User.Identity.Name, passReset.passwordOld, passReset.passwordNew))
            
                return Ok("Đổi mật khẩu thành công !");
            
            return BadRequest("Mật khẩu không chính xác hoặc lỗi !");
            
        }
        private async Task<string> CreateToken(User_Account user)
        {
            var userPermissionDTO = await _userPermissionService.GetUserPerByUserIdAsync(user.User_Id);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.User_Id),
            };
            var roles = new List<string>();
            if (userPermissionDTO != null)
            {
                foreach (User_PermissionDTO user_PermissionDTO in userPermissionDTO)
                {
                    var permission = _permissionService.GetPermissionById(user_PermissionDTO.Permission_Id);
                    if (permission != null)
                    {
                        string permissionName = permission.Permission_Name;
                        roles.Add(permissionName);
                        claims.Add(new Claim(ClaimTypes.Role, permissionName));

                    }
                }
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        /*private string CreateTokens(User_Account user)
        {
            var userPermissionDTO = _userPermissionService.GetUserPermissionByUserId(user.User_Id);
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.User_Id),
            };
            if (userPermissionDTO != null)
            {
                foreach (User_PermissionDTO user_PermissionDTO in userPermissionDTO)
                {
                    var permission = _permissionService.GetPermissionById(user_PermissionDTO.Permission_Id);
                    if (permission != null)
                    {
                        string permissionName = permission.Permission_Name;
                        claims.Add(new Claim(ClaimTypes.Role, permissionName));
                    }
                }
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }*/

    }
}
