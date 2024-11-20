using MailKit.Net.Smtp;
using MimeKit;

namespace Helper.SendEmail
{
    public class SendEmail : ISendEmail
    {
        public Task SendEmailOTP(string dest, string subject, string otp)
        {
            try
            {
                string htmlBody = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Template\\template-otp.html");
                htmlBody = htmlBody.Replace("{otp}", otp);
                // Create a new MimeMessage
                MimeMessage message = new MimeMessage();

                // Set the sender and recipient addresses
                message.From.Add(new MailboxAddress("SSPS", "sspsproject2023@gmail.com"));
                message.To.Add(new MailboxAddress("", dest));

                // Set the subject
                message.Subject = subject;

                // Create the body part of the message
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                // Attach the body to the message
                message.Body = bodyBuilder.ToMessageBody();

                // Send the email
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("sspsproject2023@gmail.com", "ujnqwamtidxlpanc");
                    client.Send(message);
                    client.Disconnect(true);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task SendEmailRef(string dest, string subject, string token)
        {
            try
            {
                string htmlBody = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Template\\forgot-password-template.html");
                htmlBody = htmlBody.Replace("{token}", token);
                // Create a new MimeMessage
                MimeMessage message = new MimeMessage();

                // Set the sender and recipient addresses
                message.From.Add(new MailboxAddress("SSPS", "sspsproject2023@gmail.com"));
                message.To.Add(new MailboxAddress("", dest));

                // Set the subject
                message.Subject = subject;

                // Create the body part of the message
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                // Attach the body to the message
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}