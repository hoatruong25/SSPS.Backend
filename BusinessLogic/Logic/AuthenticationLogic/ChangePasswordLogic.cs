using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.Security;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class ChangePasswordLogic : ILogic<ChangePasswordParam, ChangePasswordResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IJwtToken _jwtToken;
        public ChangePasswordLogic(IPgUserRepository userRepository, IJwtToken jwtToken)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
        }

        public async Task<ChangePasswordResult>? Execute(ChangePasswordParam param)
        {
            Log.Information($"ChangePasswordLogic Param: {param}");

            var returnData = new ChangePasswordResult
            {
                Result = false,
                MsgCode = "CHANGE_PASSWORD_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUser(param.Id);

                if (user == null)
                {
                    returnData.MsgDesc = "Username not found";
                    return returnData;
                }

                bool passwordCheck = BCrypt.Net.BCrypt.Verify(param.CurrentPassword, user.Password);

                if (!passwordCheck)
                {
                    returnData.MsgDesc = "Current password wrong!";
                    return returnData;
                }

                if (!param.NewPassword.Equals(param.ConfirmPassword))
                {
                    returnData.MsgDesc = "Password not match!";
                    return returnData;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(param.NewPassword);

                await _userRepository.UpdateUser(user);
                return new ChangePasswordResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    MsgDesc = "Password changed Successfully!"
                };
            }
            catch (Exception ex)
            {
                Log.Information($"ChangePassword Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}