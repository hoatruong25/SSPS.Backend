using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class GetUserDetailLogic : ILogic<GetUserDetailParam, GetUserDetailResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        public GetUserDetailLogic(IPgUserRepository userRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
        }
        public async Task<GetUserDetailResult>? Execute(GetUserDetailParam param)
        {
            Log.Information($"GetUserDetailLoigc Param: {param}");

            var returnData = new GetUserDetailResult
            {
                Result = false,
                MsgCode = "GET_USER_FAILED",
            };

            try
            {
                var resultData = await _userRepository.GetUser(param.Id);

                if (resultData == null)
                {
                    returnData.MsgCode = "USER_NOT_FOUND";
                    return returnData;
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = _autoMap.Map<PgUser, GetUserDetailDataResult>(resultData);

                return returnData;

            }
            catch (Exception ex)
            {
                Log.Information($"GetUserDetailLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}