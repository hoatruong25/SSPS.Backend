using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class GetListUserLogic : ILogic<GetListUserParam, GetListUserResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        public GetListUserLogic(IPgUserRepository userRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
        }
        public async Task<GetListUserResult>? Execute(GetListUserParam param)
        {
            Log.Information($"GetListUserLoigc Param: {param}");

            var returnData = new GetListUserResult
            {
                Result = false,
                MsgCode = "GET_USER_FAILED",
            };

            try
            {
                var resultData = await _userRepository.GetListUser(param);

                resultData.Result = true;
                resultData.MsgCode = "SUCCESS";

                return resultData;

            }
            catch (Exception ex)
            {
                Log.Information($"GetListUserLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}