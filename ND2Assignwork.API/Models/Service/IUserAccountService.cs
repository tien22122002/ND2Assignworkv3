using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service
{
    public interface IUserAccountService
    {
        Task<User_Account> GetUserAccountByIdAsync(string id);
        Task<IEnumerable<User_AccountDTO>> GetAllUserAccounts();
        Task<bool> ResetPassword(string useId, string passOld, string passNew);
        Task<bool> UpdateUserAccount(User_AccountDTO userAccountDTO);
        Task<bool> UpdateActiveUserAccount(string UserId);
        Task<bool> UpdatePsitionUserAccount(string UserId, int positionId);
        Task<bool> CreateUserAccount(User_AccountDTO userAccountDTO);




        User_AccountDTO GetUserAccountById(string id);
        User_Account GetUserAccountByUserId(string username);

        bool DeleteUserAccount(string id);
    }
}
