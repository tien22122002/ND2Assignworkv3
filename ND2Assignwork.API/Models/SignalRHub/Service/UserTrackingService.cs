namespace ND2Assignwork.API.Models.SignalRHub.Service
{
    public class UserTrackingService
{
    private readonly HashSet<string> _loggedInUsers = new();

    public void UserLoggedIn(string userId)
    {
        _loggedInUsers.Add(userId);
    }

    public void UserLoggedOut(string userId)
    {
        _loggedInUsers.Remove(userId);
    }

    public IReadOnlyCollection<string> GetLoggedInUsers()
    {
        return _loggedInUsers.ToList().AsReadOnly();
    }
}

}
