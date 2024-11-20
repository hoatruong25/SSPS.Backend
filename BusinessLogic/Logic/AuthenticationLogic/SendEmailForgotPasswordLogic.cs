using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.Security;
using Infrastructure.PgModels;
using MimeKit;
using Repository.PgReposiotries.PgForgotPasswordRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class SendEmailForgotPasswordLogic : ILogic<ForgotPasswordParam, ForgotPasswordResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgForgotPasswordRepository _forgotPasswordRepository;
        private readonly IJwtToken _jwtToken;
        public SendEmailForgotPasswordLogic(IPgUserRepository userRepository, IJwtToken jwtToken, IPgForgotPasswordRepository forgotPasswordRepository)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
            _forgotPasswordRepository = forgotPasswordRepository;
        }

        public async Task<ForgotPasswordResult>? Execute(ForgotPasswordParam param)
        {
            Log.Information($"SendEmailForgotPasswordLogic Param: {param}");

            var returnData = new ForgotPasswordResult
            {
                Result = false,
                MsgCode = "SEND_EMAIL_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUserByEmail(param.Email);

                if (user == null)
                {
                    returnData.MsgDesc = "Username not found";
                    return returnData;

                }

                //generate token
                const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var token = new string(Enumerable.Repeat(chars, 64)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var forgotPassword = await _forgotPasswordRepository.CreateForgotPasswordAsync(new PgForgotPassword
                {
                    Email = param.Email,
                    UserId = user.Id,
                    CreatorId = user.Id,
                    Token = token,
                    IsSuccess = false,
                    CreationTime = DateTime.Now,
                });

                SendEmail(param.Email, "SSPS: Reset Password", token);

                return new ForgotPasswordResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    MsgDesc = "Email sent!"
                };
            }
            catch (Exception ex)
            {
                Log.Information($"SendEmail Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }

        public void SendEmail(string to, string subject, string token)
        {
            try
            {
                string htmlBody = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Template\\forgot-password-template.html");
                htmlBody = htmlBody.Replace("{token}", token);
                // Create a new MimeMessage
                MimeMessage message = new MimeMessage();

                // Set the sender and recipient addresses
                message.From.Add(new MailboxAddress("SSPS", "sspsproject2023@gmail.com"));
                message.To.Add(new MailboxAddress("", to));

                // Set the subject
                message.Subject = subject;

                // Create the body part of the message
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;


                // Attach the body to the message
                message.Body = bodyBuilder.ToMessageBody();

                // Configure the SMTP client
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("sspsproject2023@gmail.com", "ujnqwamtidxlpanc");

                    // Send the email
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}