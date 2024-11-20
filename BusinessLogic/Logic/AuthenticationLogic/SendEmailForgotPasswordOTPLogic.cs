using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.SendEmail;
using Repository.PgReposiotries.PgOTPRepo;
using Infrastructure.PgModels;
using Serilog;
using Repository.PgReposiotries.PgUserRepo;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class SendEmailForgotPasswordOTPLogic : ILogic<ForgotPasswordOTPParam, ForgotPasswordOTPResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgOTPRepository _pgOTPRepository;
        private readonly ISendEmail _sendEmail;
        public SendEmailForgotPasswordOTPLogic(IPgUserRepository userRepository,
            IPgOTPRepository pgOTPRepository, ISendEmail sendEmail)
        {
            _userRepository = userRepository;
            _pgOTPRepository = pgOTPRepository;
            _sendEmail = sendEmail;
        }

        public async Task<ForgotPasswordOTPResult>? Execute(ForgotPasswordOTPParam param)
        {
            Log.Information($"SendEmailForgotPasswordOTPLogic Param: {param}");

            var returnData = new ForgotPasswordOTPResult
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

                //generate otp
                const string chars = "1234567890";
                var random = new Random();
                var otp = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var otpRecord = _pgOTPRepository.CreateOTP(new PgOTP
                {
                    Id = Guid.NewGuid(),
                    OTP = otp,
                    CreateAt = DateTime.Now,
                    ExpireAt = DateTime.Now.AddMinutes(30),
                    UserId = user.Id,
                    IsUsed = false,
                    Type = "FORGOT_PASSWORD"
                });

                _ = _sendEmail.SendEmailOTP(param.Email, "SSPS: Reset Password", otp);

                return new ForgotPasswordOTPResult
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
    }
}