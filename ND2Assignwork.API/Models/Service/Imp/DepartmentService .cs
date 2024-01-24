using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DataContext _context;

        public DepartmentService(DataContext context)
        {
            this._context = context;
        }

        /*public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            return _context.Department.Select(d => new DepartmentDTO
            {
                Department_ID = d.Department_ID,
                Department_Name = d.Department_Name,
                Department_Head = d.Department_Head,
                Department_Type = d.Department_Type,
            }).ToList();
        }*/
        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartments()
        {
            return await _context.Department.Select(d => new DepartmentDTO
            {
                Department_ID = d.Department_ID,
                Department_Name = d.Department_Name,
                Department_Head = d.Department_Head,
                Department_Type = d.Department_Type,
            }).ToListAsync();
        }

        public DepartmentDTO GetDepartmentById(string id)
        {
            var departmentEntity = _context.Department.Find(id);
            if (departmentEntity == null)
            {
                return null;
            }

            return new DepartmentDTO
            {
                Department_ID = departmentEntity.Department_ID,
                Department_Name = departmentEntity.Department_Name,
                Department_Head = departmentEntity.Department_Head,
                Department_Type = departmentEntity.Department_Type,
            };
        }
        public async Task<DepartmentDTO> GetDepartmentByUserIdAsync(string uid)
        {
            var departmentEntity = await _context.Department
                .Include(d => d.Users)  // Nạp thông tin về người dùng từ bảng UserAccount
                .FirstOrDefaultAsync(d => d.Users.Any(u => u.User_Id == uid));

            if (departmentEntity == null)
            {
                return null;
            }

            return new DepartmentDTO
            {
                Department_ID = departmentEntity.Department_ID,
                Department_Name = departmentEntity.Department_Name,
                Department_Head = departmentEntity.Department_Head,
                Department_Type = departmentEntity.Department_Type,
            };
        }
        public async Task<IEnumerable<DepartmentDTO>> GetDepartmentByType(int type)
        {
            var departmentEntity =await _context.Department.Where(d => d.Department_Type == type).ToListAsync();
            if (departmentEntity == null)
            {
                return null;
            }

            return departmentEntity.Select(d => new DepartmentDTO
            {
                Department_ID = d.Department_ID,
                Department_Name = d.Department_Name,
                Department_Head = d.Department_Head,
                Department_Type = d.Department_Type,
            }).ToList();
        }
        public async Task<bool> CreateDepartmentAsync(DepartmentDTO departmentDTO)
        {
            if (await _context.Department.AnyAsync(d => d.Department_ID == departmentDTO.Department_ID))
            {
                return false;
            }

            var departmentEntity = new Department
            {
                Department_ID = departmentDTO.Department_ID,
                Department_Name = departmentDTO.Department_Name,
                Department_Head = departmentDTO.Department_Head,
                Department_Type = departmentDTO.Department_Type,
            };

            await _context.Department.AddAsync(departmentEntity);

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



        public async Task<bool> UpdateDepartmentAsync(DepartmentDTO departmentDTO)
        {
            var departmentEntity = await _context.Department.FirstOrDefaultAsync(d => d.Department_ID == departmentDTO.Department_ID);
            if (departmentEntity == null)
            {
                return false;
            }

            departmentEntity.Department_Name = departmentDTO.Department_Name;
            departmentEntity.Department_Head = departmentDTO.Department_Head;
            departmentEntity.Department_Type = departmentDTO.Department_Type;

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


        public async Task<bool> UpdateDepartmentHeadAsync(string depId, string UserId)
        {
            var departmentEntity =await _context.Department.FirstOrDefaultAsync(d => d.Department_ID == depId);
            var user =await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == UserId);
            if (departmentEntity == null || user == null)
            {
                return false;
            }

            departmentEntity.Department_Head = UserId;

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

        public async Task<bool> DeleteDepartment(string id)
        {
            var departmentEntity = await _context.Department.FirstOrDefaultAsync(d => d.Department_ID == id);
            if (departmentEntity == null)
            {
                return false;
            }

            _context.Department.Remove(departmentEntity);
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
        /*public ListUserByDepartmentDTO GetUserByDepartmentId(string dep_id)
        {
            var departmentById = _context.Department.Find(dep_id);
            if (departmentById == null)
                return null;
            var userList = _context.User_Account
                .Where(u => u.User_Department == departmentById.Department_ID)
                .Select(u => new ListUserByDepartmentDTO.User
                    {
                        User_Id = u.User_Id,
                        User_FullName = u.User_FullName,
                        User_Email = u.User_Email,
                        User_Position = _context.Position
                                .Where(p => p.Position_Id == u.User_Position)
                                .Select(p => p.Position_Name)
                                .FirstOrDefault(),
                        User_Department = departmentById.Department_Name,
                        User_IsActive = u.User_IsActive,
                    }).ToList();


            var departmentDTO = new ListUserByDepartmentDTO
            {
                Department_ID = departmentById.Department_ID,
                Department_Head = departmentById.Department_Head,
                Department_Name = departmentById.Department_Name,
                Department_Type = departmentById.Department_Type,
                Users = userList,
            };

            return departmentDTO;
        }*/
        public async Task<DepartmentDTO> GetDepartmentByIdAsync(string id)
        {
            var department = await _context.Department.FirstOrDefaultAsync(d => d.Department_ID == id);
            if(department == null)
            {
                return null;
            }
            return new DepartmentDTO
            {
                Department_ID = department.Department_ID,
                Department_Name = department.Department_Name,
                Department_Head = department.Department_Head,
                Department_Type = department.Department_Type,
            };
        }

        public async Task<Department> GetListUserInDepartment(string depId)
        {
            var department = await _context.Department
                .Include(u => u.Users).ThenInclude(u => u.Position)
                .FirstOrDefaultAsync(u => u.Department_ID == depId);

            return department;
        }
        public bool isHead(string UserId)
        {
            var department = _context.Department.Find(_context.User_Account.Find(UserId).User_Department);
            if (department == null) return false;
            if (department.Department_Head.Equals(UserId)) return true;
            return false;
        }
        public async Task<bool> IsHeadyAsync(string UserId)
        {
            var User = await _context.User_Account.FirstOrDefaultAsync(u => u.User_Id == UserId);
            if (User == null || User.User_Department == null) return false;
            var department = await _context.Department.FirstOrDefaultAsync( d => d.Department_ID == User.User_Department);
            return department.Department_Head == UserId;
        }
        public async Task<bool> isHeadAsync(string UserId)
        {
            var User = await _context.User_Account.Include(u => u.Department).FirstOrDefaultAsync(u => u.User_Id == UserId);
            return (User.User_Id == User.Department.Department_Head);
        }


    }

}
