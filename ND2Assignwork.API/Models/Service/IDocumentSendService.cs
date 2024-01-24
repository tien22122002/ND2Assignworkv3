using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDocumentSendService
    {
        Task<IEnumerable<Document_SendDTO>> GetAll();

        Task<Document_Send> GetDocSendFullByDocIdAsync(string docId);
        Document_SendDTO GetById(string id);
        IEnumerable<Document_SendDTO> GetDocumentList(string user_id, bool isReceived);
        IEnumerable<Document_SendDTO> GetAllListPrivateUserSend(string user_id);
        IEnumerable<Document_SendDTO> GetAllListUserReceive(string user_id);
        IEnumerable<Document_SendDTO> GetAllListUserReceiveIsSeen(string user_id);
        bool Create(Document_SendDTO document_SendDTO);
        bool Update(Document_SendDTO document_SendDTO);
        bool UpdateState(string doc_id, int state);
        bool UpdateSeen(string doc_id, string userId);
        bool CommentEditAndStateByDocId(string docId, int state, string comment);
        bool Delete(string id);
        int GetNumberNotification(string userId);
        
        
        
        (IEnumerable<Document_SendDTO>, int) GetListByUserSendLimitNumberPage(string user_id, int limit, int numberPage);
        (IEnumerable<Document_SendDTO>, int) GetListByUserReceiveLimitNumberPage(string user_id, int limit, int numberPage);
    }
}
