using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface ITaskService
    {

        IEnumerable<TaskDTO> GetAll();
        TaskDTO GetById(string id);
        bool Create(TaskDTO taskDTO);
        bool Update(TaskDTO taskDTO);
        bool UpdateState(string taskId, int state);
        bool UpdateIsSeen(string taskId);
        bool Delete(string id);
        bool DeleteFileTask(string taskId, List<string> fileId);
        IEnumerable<TaskDTO> GetListUser(string id);
        IEnumerable<TaskDTO> GetAllTaskUserReceive(string userId);
        IEnumerable<TaskDTO> GetAllTaskUserReceiveMonth(string userId, int month, int year);
        IEnumerable<TaskDTO> GetAllTaskUserReceiveNotification(string userId);
        IEnumerable<TaskDTO> GetAllTaskUserSend(string userId);
        IEnumerable<TaskDTO> GetAllTaskDocSendId(string docId);
        float? GetPercentTaskDocSendId(string docId);


        int getNumberNotification(string userId);
    }
}
