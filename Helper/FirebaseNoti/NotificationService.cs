using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;

namespace Helper.FirebaseNoti
{
    public class NotificationService : INotificationService
    {
        public Task SendNotification(string title, string body, string deviceToken, Dictionary<string, string> extraData)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                CreateInstanceFirebase();
            }

            if (deviceToken.IsNullOrEmpty()) return Task.CompletedTask;

            // See documentation token on defining a message payload
            var message = new Message()
            {
                Data = extraData,
                Token = deviceToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            // Send a message to the device corresponding to the provided
            // Registration token.
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

            Console.WriteLine("Successfully sent message: " + response);

            return Task.CompletedTask;
        }

        public void CreateInstanceFirebase()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"JsonKey\private-key.json";
            string fullPath = Path.Combine(basePath, relativePath);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(fullPath),
            });
        }
    }
}
