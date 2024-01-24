using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using static ND2Assignwork.API.Models.DTO.ListUserByDepartmentDTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class UserAccountService : IUserAccountService
    {
        private readonly DataContext _context;

        public UserAccountService(DataContext context)
        {
            this._context = context;
        }
        public User_Account GetUserAccountByUserId(string userid)
        {
            return _context.User_Account.Where(u => u.User_Id == userid).FirstOrDefault();
        }

        public async Task<IEnumerable<User_AccountDTO>> GetAllUserAccounts()
        {
            return await _context.User_Account.Select(u => new User_AccountDTO
            {
                User_Id = u.User_Id,
                User_FullName = u.User_FullName,
                //User_Password = u.User_Password,
                User_Phone = u.User_Phone,
                User_Email = u.User_Email,
                User_Position = u.User_Position,
                User_Department = u.User_Department,
                User_IsActive = u.User_IsActive,
            }).ToListAsync();
        }

        public User_AccountDTO GetUserAccountById(string id)
        {
            var userAccountEntity = _context.User_Account.Find(id);
            if (userAccountEntity == null)
            {
                return null;
            }
            
            return new User_AccountDTO
            {
                User_Id = userAccountEntity.User_Id,
                User_FullName = userAccountEntity.User_FullName,
                //User_Password = userAccountEntity.User_Password,
                User_Phone = userAccountEntity.User_Phone,
                User_Email = userAccountEntity.User_Email,
                User_Position = userAccountEntity.User_Position,
                User_Department = userAccountEntity.User_Department,
                User_IsActive = userAccountEntity.User_IsActive,
            };
        }
        public async Task<User_Account> GetUserAccountByIdAsync(string id)
        {
            var userAccountEntity = await _context.User_Account
                .Include(u => u.Position)
                .Include(u => u.Department)
                .Include(u => u.UserPermissions).ThenInclude(u => u.Permission)
                .FirstOrDefaultAsync(u => u.User_Id == id);
            if (userAccountEntity == null)
            {
                return null;
            }
            return userAccountEntity;
            /*return new User_AccountDTOAsync
            {
                User_Id = userAccountEntity.User_Id,
                User_FullName = userAccountEntity.User_FullName,
                //User_Password = userAccountEntity.User_Password,
                User_Phone = userAccountEntity.User_Phone,
                User_Email = userAccountEntity.User_Email,
                User_Position = userAccountEntity.User_Position,
                User_Department = userAccountEntity.User_Department,
                User_IsActive = userAccountEntity.User_IsActive,
                Department = new DepartmentDTO { 
                    Department_ID = userAccountEntity.Department.Department_ID,
                    Department_Name = userAccountEntity.Department.Department_Name,
                    Department_Head = userAccountEntity.Department.Department_Head,
                    Department_Type = userAccountEntity.Department.Department_Type,
                },
                User_Permission = userAccountEntity.UserPermissions.Select(u => new User_PermissionDTO {
                    Permission_Id = u.Permission_Id
                
                }).ToList(),
            };*/
        }

        public async Task<bool> CreateUserAccount(User_AccountDTO userAccountDTO)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userAccountDTO.User_Password);
            var userAccountEntity = new User_Account
            {
                User_Id = userAccountDTO.User_Id,
                User_FullName = userAccountDTO.User_FullName,
                User_Password = hashedPassword,
                User_Phone = userAccountDTO.User_Phone,
                User_Email = userAccountDTO.User_Email,
                User_Position = userAccountDTO.User_Position,
                User_Department = userAccountDTO.User_Department,
                User_IsActive = true,
            };

            await _context.User_Account.AddAsync(userAccountEntity);

            try
            {
                int recordsAffected =await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateUserAccount(User_AccountDTO userAccountDTO)
        {
            var userAccountEntity = await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == userAccountDTO.User_Id);
            if (userAccountEntity == null)
            {
                return false;
            }

            userAccountEntity.User_FullName = userAccountDTO.User_FullName;
            userAccountEntity.User_Phone = userAccountDTO.User_Phone;
            userAccountEntity.User_Email = userAccountDTO.User_Email;
            userAccountEntity.User_Position = userAccountDTO.User_Position;
            userAccountEntity.User_Department = userAccountDTO.User_Department;
            userAccountEntity.User_IsActive = userAccountDTO.User_IsActive;

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> ResetPassword(string useId, string passOld, string passNew)
        {
            var userAccountEntity = await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == useId);
            if (userAccountEntity == null || !BCrypt.Net.BCrypt.Verify(passOld, userAccountEntity.User_Password))
            {
                return false;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passNew);
            userAccountEntity.User_Password = hashedPassword;

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateActiveUserAccount(string UserId)
        {
            var userAccountEntity = await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == UserId);
            if(userAccountEntity == null)
            {
                return false;
            }
            if(userAccountEntity.User_IsActive)
            {
                userAccountEntity.User_IsActive = false;
            }
            else
            {
                userAccountEntity.User_IsActive = true;
            }

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdatePsitionUserAccount(string UserId, int positionId)
        {
            var userAccountEntity = await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == UserId);
            if (userAccountEntity == null)
            {
                return false;
            }
            userAccountEntity.User_Position = positionId;

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }

        public bool DeleteUserAccount(string id)
        {
            var userAccountEntity = _context.User_Account.Find(id);
            if (userAccountEntity == null)
            {
                return false;
            }

            _context.User_Account.Remove(userAccountEntity);
            try
            {
                int recordsAffected = _context.SaveChanges();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
    }

}
