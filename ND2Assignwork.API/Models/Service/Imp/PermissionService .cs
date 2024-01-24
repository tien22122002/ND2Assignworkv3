using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class PermissionService : IPermissionService
    {
        private readonly DataContext _context;

        public PermissionService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<PermissionDTO> GetAllPermissions()
        {
            return _context.Permission.Select(p => new PermissionDTO
            {
                Permission_Id = p.Permission_Id,
                Permission_Name = p.Permission_Name,
            }).ToList();
        }

        public PermissionDTO GetPermissionById(int id)
        {
            var permissionEntity = _context.Permission.Find(id);
            if (permissionEntity == null)
            {
                return null;
            }

            return new PermissionDTO
            {
                Permission_Id = permissionEntity.Permission_Id,
                Permission_Name = permissionEntity.Permission_Name,
            };
        }

        public int CreatePermission(PermissionDTO_Identity permissionDTO)
        {
            var permissionEntity = new Permission
            {
                Permission_Name = permissionDTO.Permission_Name,
            };

            _context.Permission.Add(permissionEntity);
            try
            {
                _context.SaveChanges();
                return permissionEntity.Permission_Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdatePermission(PermissionDTO permissionDTO)
        {
            var permissionEntity = _context.Permission.Find(permissionDTO.Permission_Id);
            if (permissionEntity == null)
            {
                return false;
            }

            permissionEntity.Permission_Name = permissionDTO.Permission_Name;

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

        public bool DeletePermission(int id)
        {
            var permissionEntity = _context.Permission.Find(id);
            if (permissionEntity == null)
            {
                return false;
            }

            _context.Permission.Remove(permissionEntity);

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
