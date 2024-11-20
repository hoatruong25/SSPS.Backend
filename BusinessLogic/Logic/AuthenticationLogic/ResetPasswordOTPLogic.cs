using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Repository.PgReposiotries.PgOTPRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class ResetPasswordOTPLogic : ILogic<ResetPasswordOTPParam, ResetPasswordOTPResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgOTPRepository _pgOTPRepository;
        public ResetPasswordOTPLogic(IPgUserRepository userRepository, IPgOTPRepository pgOTPRepository)
        {
            _userRepository = userRepository;
            _pgOTPRepository = pgOTPRepository;
        }

        public async Task<ResetPasswordOTPResult>? Execute(ResetPasswordOTPParam param)
        {
            Log.Information($"ResetPasswordOTPLogic Param: {param}");

            var returnData = new ResetPasswordOTPResult
            {
                Result = false,
                MsgCode = "RESET_PASSWORD_FAILED",
            };

            try
            {
                var updateUser = await _userRepository.GetUserByEmail(param.Email);

                if (updateUser == null)
                {
                    returnData.MsgDesc = "User not found";
                    return returnData;
                }

                var otpRecord = _pgOTPRepository.GetOTP(updateUser.Id, param.OTP, "FORGOT_PASSWORD");

                if (otpRecord == null)
                {
                    returnData.MsgDesc = "OTP invalid";
                    return returnData;
                }

                if (otpRecord.IsUsed == true)
                {
                    returnData.MsgDesc = "OTP was used";
                    return returnData;
                }

                if (param.Password != param.ConfirmPassword)
                {
                    returnData.MsgDesc = "Password not match";
                    return returnData;
                }

                otpRecord.IsUsed = true;
                otpRecord.UseDate = DateTime.Now;
                _pgOTPRepository.UpdateOTP(otpRecord);

                updateUser.Password = BCrypt.Net.BCrypt.HashPassword(param.Password);
                updateUser.LastModificationTime = DateTime.Now;

                await _userRepository.UpdateUser(updateUser);

                return new ResetPasswordOTPResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
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