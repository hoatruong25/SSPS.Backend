using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Repository.PgReposiotries.PgUserRepo;
using Repository.Repositories.ForgotPasswordRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class ResetPasswordLogic : ILogic<ResetPasswordParam, ResetPasswordResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        public ResetPasswordLogic(IPgUserRepository userRepository, IForgotPasswordRepository forgotPasswordRepository)
        {
            _userRepository = userRepository;
            _forgotPasswordRepository = forgotPasswordRepository;
        }

        public async Task<ResetPasswordResult>? Execute(ResetPasswordParam param)
        {
            Log.Information($"ResetPasswordLogic Param: {param}");

            var returnData = new ResetPasswordResult
            {
                Result = false,
                MsgCode = "RESET_PASSWORD_FAILED",
            };

            try
            {
                var forgotPassword = await _forgotPasswordRepository.GetForgotPasswordByTokenAsync(param.Token);

                if (forgotPassword == null)
                {
                    returnData.MsgDesc = "Token invalid";
                    return returnData;
                }

                if (forgotPassword.IsSuccess == true)
                {
                    returnData.MsgDesc = "Token was used";
                    return returnData;
                }

                if (param.Password != param.ConfirmPassword)
                {
                    returnData.MsgDesc = "Password not match";
                    return returnData;
                }

                forgotPassword.IsSuccess = true;
                forgotPassword.LastModificationTime = DateTime.Now;
                await _forgotPasswordRepository.UpdateForgotPassword(forgotPassword);

                var updateUser = await _userRepository.GetUser(forgotPassword.UserId.ToString());

                if (updateUser == null)
                {
                    returnData.MsgDesc = "User not found";
                    return returnData;
                }

                updateUser.Password = BCrypt.Net.BCrypt.HashPassword(param.Password);
                updateUser.LastModificationTime = DateTime.Now;

                await _userRepository.UpdateUser(updateUser);

                return new ResetPasswordResult
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