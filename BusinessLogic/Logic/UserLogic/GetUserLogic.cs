using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class GetUserLogic : ILogic<GetUserParam, GetUserResult>
    {
        private readonly IPgUserRepository _userRepository;
        public GetUserLogic(IPgUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GetUserResult>? Execute(GetUserParam param)
        {
            Log.Information($"GetUserLogic Param: {param}");

            var returnData = new GetUserResult
            {
                Result = false,
                MsgCode = "GET_USER_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUser(param.Id);


                if (user == null)
                {
                    returnData.MsgCode = "USER_NOT_FOUND";
                    return returnData;
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new GetUserDataResult
                {
                    Id = user.Id.ToString(),
                    Code = user.Code
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetUserLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}