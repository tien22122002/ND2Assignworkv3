using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IDocumentIncommingService
    {
        Task<IEnumerable<Document_IncommingDTO>> GetAll();
        Task<Document_Incomming> GetDocInByDocInIdAsync(string docId);
        Task<Document_IncommingDTO> GetById(string id);
        Task<Document_Incomming> GetDocForwardByDocIdAsync(string docId);
        Task<IEnumerable<Document_Incomming>> GetAllListUserSend(string id);
        Task<List<Document_Incomming>> GetAllDocForwardByDocId(string docId);
        Task<IEnumerable<Document_Incomming>> GetListDocUserReceiveInDepartment(string user_id);
        Task<IEnumerable<Document_Incomming>> GetListDocUserReceiveOutDepartment(string user_id);
        Task<IEnumerable<Document_Incomming>> GetListByUserReceiveIsSeen(string user_id);
        Task<bool> Update(Document_IncommingDTO Document_IncommingDTO);
        Task<bool> UpdateStateComment(string doc_id, int state, string comment);
        Task<bool> UpdateSeenTrue(string doc_id);
        Task<bool> Delete(string id);
        Task<bool> Create(Document_IncommingDTO document_IncommingDTO);
        Task<bool> UpdateState(string doc_id, int state);








        (IEnumerable<Document_IncommingDTO>, int) GetListByUserSendLimitNumberPage(string user_id, int limit, int numberPage);
        (IEnumerable<Document_IncommingDTO>, int) GetListByUserReceiveLimitNumberPage(string user_id, int limit, int numberPage);
        //bool SetHandOverDocument(string doc_id, List<string> user_id);
        bool UpdateHandOverDocument(string doc_id, List<string> user_id);
        bool isExist(string id);
        int getNumberNotification(string userId);


    }
}
