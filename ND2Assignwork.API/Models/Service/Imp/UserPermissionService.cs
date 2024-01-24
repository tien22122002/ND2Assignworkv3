using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly DataContext _context;

        public UserPermissionService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<User_PermissionDTO> GetAllUserPermissions()
        {
            return _context.User_Permission.Select(u => new User_PermissionDTO
            {
                
                User_Id = u.User_Id,
                Permission_Id = u.Permission_Id,
            }).ToList();
        }
        public IEnumerable<User_PermissionDTO> GetUserPermissionByUserId(string user_id)
        {
            var userPermissionEntities = _context.User_Permission
                .Where(up => up.User_Id == user_id)
                .ToList();

            if (userPermissionEntities == null || userPermissionEntities.Count == 0)
            {
                return null;
            }

            var userPermissionDTOs = userPermissionEntities
                .Select(up => new User_PermissionDTO
                {
                    User_Id = up.User_Id,
                    Permission_Id = up.Permission_Id,
                });

            return userPermissionDTOs;
        }
        public async Task<IEnumerable<User_PermissionDTO>> GetUserPerByUserIdAsync(string user_id)
        {
            var userPermissionEntities = await _context.User_Permission
                .Where(up => up.User_Id == user_id)
                .ToListAsync();

            if (userPermissionEntities == null || userPermissionEntities.Count == 0)
            {
                return null;
            }

            var userPermissionDTOs = userPermissionEntities
                .Select(up => new User_PermissionDTO
                {
                    User_Id = up.User_Id,
                    Permission_Id = up.Permission_Id,
                });

            return userPermissionDTOs;
        }


        public User_PermissionDTO GetUserPermissionById(string user_id, int per_id)
        {
            var userPermissionEntity = _context.User_Permission.Find(user_id, per_id);
            if (userPermissionEntity == null)
            {
                return null;
            }

            return new User_PermissionDTO
            {
                User_Id = userPermissionEntity.User_Id,
                Permission_Id = userPermissionEntity.Permission_Id,
            };
        }

        
        public async Task<bool> CreateUserPermissionAsync(User_PermissionDTO userPermissionDTO)
        {
            var userPermissionEntity = new User_Permission
            {
                User_Id = userPermissionDTO.User_Id,
                Permission_Id = userPermissionDTO.Permission_Id,
            };

            await _context.User_Permission.AddAsync(userPermissionEntity);
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
        public async Task<bool> DeleteUserPermissionAsync(string user_id, int per_id)
        {
            var userPermissionEntity = await _context.User_Permission.FirstOrDefaultAsync(u => u.User_Id == user_id && u.Permission_Id == per_id);
            if (userPermissionEntity == null)
            {
                throw new ArgumentException("User permission not found");
            }

            _context.User_Permission.Remove(userPermissionEntity);

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

        public bool DeleteUserPermission(string user_id, int per_id)
        {
            var userPermissionEntity = _context.User_Permission.Find(user_id, per_id);
            if (userPermissionEntity == null)
            {
                throw new ArgumentException("User permission not found");
            }

            _context.User_Permission.Remove(userPermissionEntity);

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
        public bool isExistPer(int per_id)
        {

            bool exists = _context.User_Permission.Any(d => d.Permission_Id == per_id);
            return exists;

        }
        public bool isExistUser(string user_id)
        {
            bool exists = _context.User_Permission.Any(d => d.User_Id == user_id);
            return exists;
        }
    }

}
