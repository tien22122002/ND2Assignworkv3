using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDiscussService
    {
        IEnumerable<DiscussDTO> GetAll();
        IEnumerable<DiscussDTO> GetByTaskId(string id, string userid);
        IEnumerable<DiscussDTO> GetListDiscussByUsserNotification(string userId);
        DiscussDTO GetDiscuss(string TaskId, string UserId, DateTime time);
        bool Create(DiscussDTO discussDTO);
        bool Update(DiscussDTO discussDTO);


        bool DeleteDiscuss(string taskId, string UserId, DateTime time);
        bool DeleteAllDiscussByTaskId(string taskId);

        bool isExistTask(string task_id);

        int GetNumberNitification(string userId);

    }
}
