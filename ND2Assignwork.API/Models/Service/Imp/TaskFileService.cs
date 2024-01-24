using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class TaskFileService : ITaskFileService
    {
        private readonly DataContext _context;

        public TaskFileService(DataContext dataContext)
        {
            this._context = dataContext;
        }

        public IEnumerable<Task_FileDTO> GetAllTaskFile()
        {
            return _context.Task_File.Select(u => new Task_FileDTO
            {
                File_Id = u.File_Id,
                Task_Id = u.Task_Id,
            }).ToList();
        }
        public Task_FileDTO GetOneTaskFile(string task_id, string file_id)
        {
            var taskFileEntity = _context.Task_File.Find(task_id, file_id);
            if (taskFileEntity == null)
            {
                return null;
            }

            return new Task_FileDTO
            {
                File_Id = taskFileEntity.File_Id,
                Task_Id = taskFileEntity.Task_Id
            };
        }
        public bool CreateTaskFile(Task_FileDTO task_FileDTO)
        {
            var taskFileEntity = new Task_File
            {
                File_Id = task_FileDTO.File_Id,
                Task_Id = task_FileDTO.Task_Id,
                User_Id = task_FileDTO.User_Id,
            };

            _context.Task_File.Add(taskFileEntity);
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
        public bool DeleteTaskFile(string task_id, string file_id)
        {
            var taskFileEntity = _context.Task_File.Find(file_id, task_id);
            if (taskFileEntity == null)
            {
                throw new ArgumentException("Task File not found");
            }

            _context.Task_File.Remove(taskFileEntity);

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
        public bool DeleteTaskFilesByTaskId(string task_id)
        {
            var taskFileEntities = _context.Task_File.Where(tf => tf.Task_Id == task_id).ToList();

            if (taskFileEntities.Count == 0)
            {
                return false; 
            }

            _context.Task_File.RemoveRange(taskFileEntities);

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
        public bool DeleteTaskFilesByFileId(string file_id)
        {
            var taskFileEntities = _context.Task_File.Where(tf => tf.File_Id == file_id).ToList();

            if (taskFileEntities.Count == 0)
            {
                return false; 
            }

            _context.Task_File.RemoveRange(taskFileEntities);

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


        public bool isExistTask(string task_id)
        {
            
            bool exists = _context.Task_File.Any(d => d.Task_Id == task_id);
            return exists;
            
        }
        public bool isExistFile(string file_id)
        {
            bool exists = _context.Task_File.Any(d => d.File_Id == file_id);
            return exists;
        }
        public IEnumerable<Task_FileDTO> GetTaskFileByTaskId(string task_id)
        {
            var taskFileEntity = _context.Task_File
                .Where(up => up.Task_Id == task_id)
                .ToList();

            if (taskFileEntity == null || taskFileEntity.Count == 0)
            {
                return null;
            }

            var task_FileDTOs = taskFileEntity
                .Select(up => new Task_FileDTO
                {
                    File_Id = up.File_Id,
                    Task_Id = up.Task_Id,
                });

            return task_FileDTOs;
        }
    }
}
