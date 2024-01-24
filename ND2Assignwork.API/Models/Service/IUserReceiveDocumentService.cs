using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IUserReceiveDocumentService
    {
        IEnumerable<User_Receive_DocumentDTO> GetAllUserReceice();
        User_Receive_DocumentDTO GetOneUserReceice(string user_id, string doc_id);
        bool CreateUserReceice(User_Receive_DocumentDTO user_Receive_DocumentDTO);
        bool DeleteUserReceice(string user_id, string doc_id);
        IEnumerable<User_Receive_DocumentDTO> GetUserReceiceByUserId(string user_id);
        IEnumerable<User_Receive_DocumentDTO> GetUserReceiceByDocId(string doc_id);
        bool isExistDocSend(string doc_id);
        bool isExistUser(string user_id);
        bool DeleteUserReceivesByDocId(string doc_id);
    }
}
