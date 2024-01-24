using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDocumentSendFileService
    {
        IEnumerable<Document_Send_FileDTO> GetAllDocumentSendFile();
        Document_Send_FileDTO GetOneDocSendFile(string doc_id, string file_id);
        bool CreateDocSendFile(Document_Send_FileDTO document_Send_FileDTO);
        bool DeleteDocSendFile(string doc_id, string file_id);
        IEnumerable<Document_Send_FileDTO> GetDocSendFileByDocId(string doc_id);

        bool isExistDocSend(string doc_id);
        bool isExistFile(string file_id);
        bool DeleteDocSendFilesByFileId(string file_id);
        bool DeleteDocSendFilesByDocId(string doc_id);
    }
}
