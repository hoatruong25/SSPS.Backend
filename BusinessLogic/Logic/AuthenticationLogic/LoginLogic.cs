using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.Security;
using Microsoft.IdentityModel.Tokens;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class LoginLogic : ILogic<LoginParam, LoginResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IJwtToken _jwtToken;
        public LoginLogic(IPgUserRepository userRepository, IJwtToken jwtToken)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
        }

        public async Task<LoginResult>? Execute(LoginParam param)
        {
            Log.Information($"LoginLogic Param: {param}");

            var returnData = new LoginResult
            {
                Result = false,
                MsgCode = "LOGIN_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUserByEmail(param.Email);

                if (user == null)
                {
                    returnData.MsgDesc = "Username not found";
                    return returnData;
                }

                bool passwordCheck = BCrypt.Net.BCrypt.Verify(param.Password, user.Password);

                if (!passwordCheck)
                {
                    returnData.MsgCode = "Password not match";
                    return returnData;
                }

                var gToken = _jwtToken.GenerateJwtToken(user.Id.ToString(), user.Code, user.Email, user.Role, user.FirstName, user.LastName);

                if (user == null) return returnData;

                // Add device token to database
                if (!param.DeviceToken.IsNullOrEmpty())
                {
                    user.DeviceToken = param.DeviceToken;
                    await _userRepository.UpdateUser(user);
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new LoginDataResult
                {
                    AccessToken = gToken.AccessToken,
                    RefreshToken = gToken.RefreshToken
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"LoginLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}