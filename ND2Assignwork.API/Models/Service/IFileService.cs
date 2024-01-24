using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IFileService
    {
        Task<FileDTO> GetFileById(string id);
        Task<List<Domain.File>> FileNotConect();
        
        Task<IEnumerable<FileDTO>> GetFileByDocInIdAsync(string id);
        Task<IEnumerable<FileDTO>> GetFileByDocSendIdAsync(string id);
        Task<IEnumerable<FileDTO>> GetFileByTaskIdAsync(string id);
        Task<IEnumerable<FileDTO>> GetFileByDocSendIdComfirmAsync(string id);
        Task<IEnumerable<FileDTO>> GetFileByTaskIdCofirmAsync(string id);
        Task<bool> CreateAsync(FileDTO fileDTO);
        bool Create(FileDTO fileDTO);


        IEnumerable<FileDTO> GetFileByDocId(string id);
        IEnumerable<FileDTO> GetFileByDocSendId(string id);
        IEnumerable<FileDTO> GetFileByTaskId(string id);
        IEnumerable<FileDTO> GetFileByTaskIdcComfirm(string id);
        IEnumerable<FileDTO> GetFileByDocSendIdComfirm(string id);

        
        
        IEnumerable<FileDTO> GetAll();
        FileDTO GetFileDocInById(string id, string userId);
        FileDTO GetFileDocSendById(string id, string userId);
        FileDTO GetFileTaskById(string id, string userId);
        bool Update(FileDTO fileDTO);
        bool Delete(string id);
        bool DeleteListFileNotConnect(List<Domain.File> fileIds);
    }
}
