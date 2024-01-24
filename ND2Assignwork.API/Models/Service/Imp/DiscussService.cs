using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using System.Threading.Tasks;
using static ND2Assignwork.API.Models.DTO.ListUserByDepartmentDTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DiscussService : IDiscussService
    {
        private readonly DataContext _context;

        public DiscussService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<DiscussDTO> GetAll()
        {
            return _context.Discuss.Select(p => new DiscussDTO
            {
                Discuss_Task = p.Discuss_Task,
                Discuss_User = p.Discuss_User,
                Discuss_Time = p.Discuss_Time,
                Discuss_Content = p.Discuss_Content,
            }).ToList();
        }
        public IEnumerable<DiscussDTO> GetByTaskId(string TaskId, string userId)
        {
            var task = _context.Task.Where(t => t.Task_Id == TaskId).FirstOrDefault();
            if (task == null)
            {
                return null;
            }
            if (task.Task_Person_Send != userId && task.Task_Person_Receive != userId)
                return null;

            var discusseList = _context.Discuss
                .Where(up => up.Discuss_Task == TaskId)
                .OrderBy(p => p.Discuss_Time)
                .ToList();
            if (discusseList == null || discusseList.Count == 0)
            {
                return null;
            }
            var discussDTO = discusseList
                .Select(up => new DiscussDTO
                {
                    Discuss_Task = up.Discuss_Task,
                    Discuss_User = up.Discuss_User,
                    Discuss_Time= up.Discuss_Time,
                    Discuss_Content= up.Discuss_Content,
                });

            return discussDTO;
        }
        public IEnumerable<DiscussDTO> GetListDiscussByUsserNotification(string userId)
        {
            var tasks = _context.Task.Where(t => t.Task_Person_Send == userId || t.Task_Person_Receive == userId).ToList();

            if (!tasks.Any())
                return null;

            var listDiscuss = tasks.SelectMany(t =>
                _context.Discuss
                    .Where(up => up.Discuss_Task == t.Task_Id && up.Discuss_IsSeen == false && up.Discuss_User != userId)
            );

            /*var listDiscuss = new List<Discuss>();
            foreach (var t in tasks)
            {
                var Dis = _context.Discuss
                    .Where(up => up.Discuss_Task == t.Task_Id*//* && up.Discuss_IsSeen == false && up.Discuss_User != userId*//*).ToList();
                if(Dis.Count()  > 0)
                {
                    foreach (var d in Dis)
                    {
                        if (d != null)
                        {
                            listDiscuss.Add(d);
                        }
                    }
                }
                
            }*/

            if (!listDiscuss.Any())
                return null;

            var discussDTO = listDiscuss
                .OrderByDescending(d => d.Discuss_Time)
                .Select(up => new DiscussDTO
                {
                    Discuss_Task = up.Discuss_Task,
                    Discuss_User = up.Discuss_User,
                    Discuss_Time = up.Discuss_Time,
                    Discuss_Content = up.Discuss_Content,
                });

            return discussDTO;
        }
        public int GetNumberNitification(string userId)
        {
            var tasks = _context.Task.Where(t => t.Task_Person_Send == userId || t.Task_Person_Receive == userId).ToList();

            if (!tasks.Any())
                return 0;

            var listDiscuss = tasks.SelectMany(t =>
            _context.Discuss
                    .Where(up => up.Discuss_Task == t.Task_Id && up.Discuss_IsSeen == false && up.Discuss_User != userId)
            ).ToList();
            return listDiscuss.Count();
        }

        public DiscussDTO GetDiscuss(string taskId, string userId, DateTime time)
        {
            var discussEntity = _context.Discuss.Find(taskId, userId, time);
            if (discussEntity == null)
            {
                return null;
            }
            return new DiscussDTO
            {
                Discuss_Task = discussEntity.Discuss_Task,
                Discuss_User = discussEntity.Discuss_User,
                Discuss_Time = discussEntity.Discuss_Time,
                Discuss_Content = discussEntity.Discuss_Content,
            };
        }


        public bool Create(DiscussDTO p)
        {
            var discussEntity = new Domain.Discuss
            {
                Discuss_Task = p.Discuss_Task,
                Discuss_User = p.Discuss_User,
                Discuss_Time = p.Discuss_Time,
                Discuss_Content = p.Discuss_Content,
                Discuss_IsSeen = p.Discuss_IsSeen,
            };
            _context.Discuss.Add(discussEntity);

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
        public bool Update(DiscussDTO discussDTO)
        {
            var discussEntity = _context.Discuss.Where(d => d.Discuss_Task == discussDTO.Discuss_Task && d.Discuss_User == d.Discuss_User).ToList();
            if (discussEntity == null)
            {
                return false;
            }
            if(discussEntity.Count() > 0)
            {
                foreach (var d in discussEntity)
                {
                    d.Discuss_IsSeen = true;
                }
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
            return false;
            
        }
        
        public bool DeleteDiscuss(string taskId, string userId, DateTime time)
        {
            var discussEntity = _context.Discuss.Find(taskId, userId, time);
            if (discussEntity == null)
            {
                return false;
            }
            _context.Discuss.Remove(discussEntity);

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

            bool exists = _context.Discuss.Any(d => d.Discuss_Task == task_id);
            return exists;

        }
        public bool DeleteAllDiscussByTaskId(string taskId)
        {
            var discussEntities = _context.Discuss.Where(d => d.Discuss_Task == taskId).ToList();

            if (discussEntities.Count == 0)
            {
                return false;
            }

            _context.Discuss.RemoveRange(discussEntities);

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
