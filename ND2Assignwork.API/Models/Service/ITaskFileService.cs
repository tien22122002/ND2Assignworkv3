using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface ITaskFileService
    {
        IEnumerable<Task_FileDTO> GetAllTaskFile();
        Task_FileDTO GetOneTaskFile(string task_id, string file_id);
        bool CreateTaskFile(Task_FileDTO task_FileDTO);
        bool DeleteTaskFile(string task_id, string file_id);
        IEnumerable<Task_FileDTO> GetTaskFileByTaskId(string task_id);

        bool isExistTask(string task_id);
        bool isExistFile(string file_id);
        bool DeleteTaskFilesByTaskId(string task_id);
        bool DeleteTaskFilesByFileId(string file_id);
    }
}
