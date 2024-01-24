using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;

        public TaskService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<TaskDTO> GetAll()
        {
            return _context.Task.Select(p => new TaskDTO
            {
                Task_Id = p.Task_Id,
                Task_Title = p.Task_Title,
                Task_Content = p.Task_Content,
                Task_Category = p.Task_Category,
                Task_DateStart = p.Task_TimeStart,
                Task_DateSend = p.Task_DateSend,
                Task_DateEnd = p.Task_DateEnd,
                Task_Person_Send = p.Task_Person_Send,
                Task_Person_Receive = p.Task_Person_Receive,
                Task_State = p.Task_State,
                Document_Send_Id = p.Document_Send_Id,
            }).ToList();
        }
        public TaskDTO GetById(string id)
        {
            var p = _context.Task.Find(id);
            if (p == null)
            {

            }
            return new TaskDTO
            {
                Task_Id = p.Task_Id,
                Task_Title = p.Task_Title,
                Task_Content = p.Task_Content,
                Task_Category = p.Task_Category,
                Task_DateSend = p.Task_DateSend,
                Task_DateStart = p.Task_TimeStart,
                Task_DateEnd = p.Task_DateEnd,
                Task_Person_Send = p.Task_Person_Send,
                Task_Person_Receive = p.Task_Person_Receive,
                Task_State = p.Task_State,
                Document_Send_Id = p.Document_Send_Id,
                Task_TimeUpdate = p.Task_TimeUpdate,
            };
        }
        public IEnumerable<TaskDTO> GetAllTaskUserSend(string userId)
        {
            var taskSendEntities = _context.Task
                   .Where(up => up.Task_Person_Send == userId)
                   .OrderByDescending(p => p.Task_DateSend);
            if(taskSendEntities.Count() <= 0)
                return new List<TaskDTO>();
            var taskSendDTOs = taskSendEntities
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_Person_Receive = p.Task_Person_Receive,
                    Task_State = p.Task_State,
                    Document_Send_Id = p.Document_Send_Id,
                })
                .ToList();

            return taskSendDTOs;
        }
        public IEnumerable<TaskDTO> GetAllTaskUserReceive(string userId)
        {
            var taskSendEntities = _context.Task
                   .Where(up => up.Task_Person_Receive == userId)
                   .OrderByDescending(p => p.Task_DateSend);
            if (taskSendEntities.Count() <= 0)
                return new List<TaskDTO>();
            var taskSendDTOs = taskSendEntities
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_Person_Receive = p.Task_Person_Receive,
                    Task_State = p.Task_State,
                    Document_Send_Id = p.Document_Send_Id,
                })
                .ToList();

            return taskSendDTOs;
        }

        public IEnumerable<TaskDTO> GetAllTaskUserReceiveMonth(string userId, int month, int year)
        {
            var taskEntities = _context.Task
                .Where(up => up.Task_Person_Receive == userId &&
                             up.Task_DateSend.Month == month &&
                             up.Task_DateSend.Year == year)
                .OrderByDescending(p => p.Task_DateSend);

            if (taskEntities.Count() <= 0)
                return new List<TaskDTO>();

            var taskDTOs = taskEntities
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_Person_Receive = p.Task_Person_Receive,
                    Task_State = p.Task_State,
                    Document_Send_Id = p.Document_Send_Id,
                })
                .ToList();

            return taskDTOs;
        }


        public IEnumerable<TaskDTO> GetAllTaskUserReceiveNotification(string userId)
        {
            var taskSendEntities = _context.Task
                  .Where(up => up.Task_Person_Receive == userId && up.Task_IsSeen == false && (up.Task_State == 3 || up.Task_State == 4)).ToList();
            var taskSendEntitiesSend = _context.Task
                   .Where(up => up.Task_Person_Send == userId && up.Task_IsSeen == false && up.Task_State == 5).ToList();
            foreach ( var entity in taskSendEntitiesSend )
            {
                taskSendEntities.Add( entity );
            }
            if (taskSendEntities.Count == 0 || taskSendEntities == null)
                return new List<TaskDTO>();
            var taskSendDTOs = taskSendEntities
                .OrderByDescending(p => p.Task_DateSend)
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_Person_Receive = p.Task_Person_Receive,
                    Task_State = p.Task_State,
                    Document_Send_Id = p.Document_Send_Id,
                    Task_TimeUpdate =  p.Task_TimeUpdate,
                })
                .ToList();

            return taskSendDTOs;
        }
        public int getNumberNotification(string userId)
        {
            var taskSendEntities = _context.Task
                   .Where(up => up.Task_Person_Receive == userId && up.Task_IsSeen == false && (up.Task_State == 3 || up.Task_State == 4)).ToList();
            var taskSendEntitiesSend = _context.Task
                   .Where(up => up.Task_Person_Send == userId && up.Task_IsSeen == false && up.Task_State == 5).ToList();
            foreach (var entity in taskSendEntitiesSend)
            {
                taskSendEntities.Add(entity);
            }
            if (taskSendEntities.Count <= 0 || taskSendEntities == null)
                return 0;
            return taskSendEntities.Count();
        }
        public IEnumerable<TaskDTO> GetAllTaskDocSendId(string docId)
        {
            var taskSendEntities = _context.Task
                   .Where(up => up.Document_Send_Id == docId);
            if (taskSendEntities.Count() <= 0)
                return new List<TaskDTO>();
            var taskSendDTOs = taskSendEntities
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_Person_Receive = p.Task_Person_Receive,
                    Task_State = p.Task_State,
                    Document_Send_Id = p.Document_Send_Id,
                })
                .ToList();

            return taskSendDTOs;
        }
        public float? GetPercentTaskDocSendId(string docId)
        {
            var taskSendEntities = _context.Task
                   .Where(up => up.Document_Send_Id == docId);
            var taskOver = 0;
            var taskSent = 0;
            foreach(var task in taskSendEntities)
            {
                if(task.Task_State == 5)
                {
                    taskOver++;
                }
                taskSent++;
            }
            if (taskSent > 0)
            {
                if (taskOver == 0)
                    return 0;
                float taskPercent = (((float)taskOver / (float)taskSent) * 100);
                return taskPercent;
            }
            return (float?)null;
        }
        public bool Create(TaskDTO p)
        {
            var taskEntity = new Domain.Task 
            {
                Task_Id = p.Task_Id,
                Task_Title = p.Task_Title,
                Task_Content = p.Task_Content,
                Task_Category = p.Task_Category,
                Task_TimeStart = p.Task_DateStart,
                Task_DateSend = p.Task_DateSend,
                Task_DateEnd = p.Task_DateEnd,
                Task_Person_Send = p.Task_Person_Send,
                Task_Person_Receive= p.Task_Person_Receive,
                Task_State = p.Task_State,
                Document_Send_Id= p.Document_Send_Id,
            };
            _context.Task.Add(taskEntity);

            int recordsAffected = _context.SaveChanges();
            return recordsAffected > 0;
        }
        public bool Update(TaskDTO taskDTO)
        {
            var taskEntity = _context.Task.Find(taskDTO.Task_Id);
            if (taskEntity == null)
            {
                return false;
            }
            taskEntity.Task_Title = taskDTO.Task_Title;
            taskEntity.Task_Content = taskDTO.Task_Content;
            taskEntity.Task_Category = taskDTO.Task_Category;
            taskEntity.Task_DateSend = taskDTO.Task_DateSend;
            taskEntity.Task_TimeStart = taskDTO.Task_DateStart;
            taskEntity.Task_DateEnd  = taskDTO.Task_DateEnd;
            taskEntity.Task_State = taskDTO.Task_State;
            taskEntity.Task_IsSeen = taskDTO.Task_IsSeen;


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
        public bool UpdateState(string taskId, int state)
        {
            var taskEntity = _context.Task.Find(taskId);
            if (taskEntity == null)
            {
                return false;
            }
            taskEntity.Task_State = state;
            taskEntity.Task_TimeUpdate = DateTime.Now;
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
        public bool UpdateIsSeen(string taskId)
        {
            var taskEntity = _context.Task.Find(taskId);
            if (taskEntity == null)
            {
                return false;
            }
            taskEntity.Task_IsSeen = true;
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
        public bool Delete(string id)
        {
            var taskEntity = _context.Task.Find(id);
            if (taskEntity == null)
            {
                return false;
            }
            _context.Task.Remove(taskEntity);

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
        public bool DeleteFileTask(string taskId, List<string> fileIds)
        {
            var listFileOld = _context.Task_File.Where(t => t.Task_Id == taskId).ToList();

            foreach (var file in listFileOld)
            {
                if (!fileIds.Contains(file.File_Id))
                {
                    _context.Task_File.Remove(file);
                }
            }
            _context.SaveChanges();
            return true;
        }


        public IEnumerable<TaskDTO> GetListUser(string id)
        {
            
            var userTaskInEntities = _context.Task
                .Where(up => up.Task_Person_Send == id)
                .ToList();

            if (userTaskInEntities == null || userTaskInEntities.Count == 0)
            {
                return null;
            }

            var userTaskInDTOs = userTaskInEntities
                .Select(p => new TaskDTO
                {
                    Task_Id = p.Task_Id,
                    Task_Title = p.Task_Title,
                    Task_Content = p.Task_Content,
                    Task_Category = p.Task_Category,
                    Task_DateSend = p.Task_DateSend,
                    Task_DateStart = p.Task_TimeStart,
                    Task_DateEnd = p.Task_DateEnd,
                    Task_Person_Send = p.Task_Person_Send,
                    Task_State = p.Task_State,
                });

            return userTaskInDTOs;
        }
        
    }
}
