using DTO.Const;
using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using Helper.SendEmail;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgOTPRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class RegisterOTPLogic : ILogic<RegisterOTPParam, RegisterOTPResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        private readonly ISendEmail _sendEmail;
        private readonly IPgOTPRepository _pgOTPRepository;
        public RegisterOTPLogic(IPgUserRepository userRepository, IAutoMap autoMap, ISendEmail sendEmail, IPgOTPRepository pgOTPRepository)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
            _sendEmail = sendEmail;
            _pgOTPRepository = pgOTPRepository;
        }

        public async Task<RegisterOTPResult>? Execute(RegisterOTPParam param)
        {
            Log.Information($"RegisterOTPLogic Param: {param}");

            var returnData = new RegisterOTPResult
            {
                Result = false,
                MsgCode = "REGISTER_FAILED",
            };

            try
            {
                var isEmailExit = await _userRepository.IsEmailExit(param.Email);
                if (isEmailExit)
                {
                    returnData.MsgDesc = "Email was exist";
                    return returnData;
                }

                var isCodeExit = await _userRepository.IsCodeExit(param.Email);
                if (isCodeExit)
                {
                    returnData.MsgDesc = "Code was exist";
                    return returnData;
                }

                var userCreate = _autoMap.Map<RegisterOTPParam, PgUser>(param);

                userCreate.Id = Guid.NewGuid();
                userCreate.Password = BCrypt.Net.BCrypt.HashPassword(param.Password);
                userCreate.CreationTime = DateTime.Now;
                userCreate.Status = UserConst.USER_STATUS_BLOCK;

                var user = _userRepository.CreateUser(userCreate);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new RegisterOTPDataResult
                {
                    Id = userCreate.Id.ToString()
                };

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
                    UserId = userCreate.Id,
                    IsUsed = false,
                    Type = "REGISTER"
                });

                _ = _sendEmail.SendEmailOTP(param.Email, "REGISTER", otp);

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"RegisterLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}