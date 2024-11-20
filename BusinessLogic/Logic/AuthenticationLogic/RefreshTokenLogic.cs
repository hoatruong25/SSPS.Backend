using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.Security;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class RefreshTokenLogic : ILogic<RefreshTokenParam, RefreshTokenResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IJwtToken _jwtToken;
        public RefreshTokenLogic(IPgUserRepository userRepository, IJwtToken jwtToken)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
        }

        public async Task<RefreshTokenResult>? Execute(RefreshTokenParam param)
        {
            Log.Information($"RefreshTokenLogic Param: {param}");

            var returnData = new RefreshTokenResult
            {
                Result = false,
                MsgCode = "REFRESH_TOKEN_FAILED",
            };

            try
            {
                var verifyToken = _jwtToken.VerifyToken(param.RefreshToken);

                if (verifyToken?.Id != null)
                {
                    var user = await _userRepository.GetUser(verifyToken.Id);

                    if (user == null)
                        return returnData;

                    var genToken = _jwtToken.GenerateJwtToken(user.Id.ToString(), user.Code, user.Email, user.Role, user.FirstName, user.LastName);

                    returnData.Result = true;
                    returnData.MsgCode = "SUCCESS";
                    returnData.Data = new RefreshTokenDataResult
                    {
                        AccessToken = genToken.AccessToken
                    };
                    return returnData;
                }

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"RefreshTokenLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}