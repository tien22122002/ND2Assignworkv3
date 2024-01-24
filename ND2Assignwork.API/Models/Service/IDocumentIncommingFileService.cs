using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDocumentIncommingFileService
    {
        IEnumerable<Document_Incomming_FileDTO> GetAllDocumentFile();
        Document_Incomming_FileDTO GetOneDocFile(string doc_id, string file_id);
        bool DeleteDocFile(string doc_id, string file_id);



        Task<bool> CreateDocFile(Document_Incomming_FileDTO document_Incomming_FileDTO);
        Task<IEnumerable<Document_Incomming_FileDTO>> GetDocFileByDocId(string doc_id);
        Task<bool> DeleteDocFilesByDocId(string doc_id);
        
        
        bool isExistDocIn(string doc_id);
        bool isExistFile(string file_id);
        bool DeleteDocFilesByFileId(string file_id);
        IEnumerable<Document_Incomming_FileDTO> GetAllDocByDocId(string doc_id);
    }
}
