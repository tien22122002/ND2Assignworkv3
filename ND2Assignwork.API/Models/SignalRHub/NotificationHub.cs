using Microsoft.AspNetCore.SignalR;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.SignalRHub.Service;
using static ND2Assignwork.API.Models.DTO.ListUserByDepartmentDTO;

namespace ND2Assignwork.API.Models.SignalRHub
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly UserTrackingService _userTrackingService;
        private readonly DataContext _dataContext;

        public NotificationHub(ILogger<NotificationHub> logger, UserTrackingService userTrackingService, DataContext dataContext)
        {
            _logger = logger;
            _userTrackingService = userTrackingService;
            _dataContext = dataContext;
        }

        public async Task AddToGroup(string userId)
        {
            var userExists = _dataContext.User_Account.Any(u => u.User_Id == userId);

            if (userExists)
            {
                _userTrackingService.UserLoggedIn(userId); 
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                //_logger.LogInformation($"User {userId} added to group.");
                await Clients.Group("admin").SendAsync("ReceiveMessage", $"User {userId} đã đăng nhập.");
            }
            else
            {
                _logger.LogError($"User {userId} does not exist or is not authenticated.");
            }
        }

        public async Task RemoveFromGroup(string userId)
        {
            _userTrackingService.UserLoggedOut(userId); // Loại bỏ người dùng khỏi danh sách đăng nhập
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            _logger.LogInformation($"User {userId} removed from group.");
        }

        public IReadOnlyCollection<string> GetLoggedInUsers()
        {
            return _userTrackingService.GetLoggedInUsers();
        }
        public async Task UpdateStatus(object message, IList<string> userIds)
        {
            foreach (var user in userIds)
            {
                await Clients.Group(user).SendAsync("StatusUpdate", message);
            }
        }
        public async Task SendUpdateNotification(object message, string user)
        {
            try
            {
                await Clients.Group(user).SendAsync("ReceiveUpdateNotification", message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending message: {Error}", ex.Message);
            }
        }
        public async Task SendDiscussNotification(object message, string user)
        {
            try
            {
                await Clients.Group(user).SendAsync("ReceiveDiscussNotification", message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending message: {Error}", ex.Message);
            }
        }

    }
}
