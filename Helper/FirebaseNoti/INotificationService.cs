using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Helper.FirebaseNoti
{
    public interface INotificationService
    {
        Task SendNotification(string title, string body, string deviceToken, Dictionary<string, string> extraData);
    }
}
