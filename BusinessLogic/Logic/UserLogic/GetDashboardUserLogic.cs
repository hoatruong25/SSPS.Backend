using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class GetDashboardUserLogic : ILogic<DashboardUserParam, DashboardUserResult>
    {
        private readonly IPgUserRepository _pgUserRepository;

        public GetDashboardUserLogic(IPgUserRepository pgUserRepository)
        {
            _pgUserRepository = pgUserRepository;
        }

        public async Task<DashboardUserResult>? Execute(DashboardUserParam param)
        {
            Log.Information($"GetDashboardLogic Param: {param}");

            var returnData = new DashboardUserResult
            {
                Result = false,
                MsgCode = "GET_DASHBOARD_FAILED",
            };

            try
            {
                returnData.Data = _pgUserRepository.GetDashboard(param.UserId, param.Type, param.FromDate, param.ToDate);

                if (returnData.Data == null)
                {
                    return returnData;
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetDashboardResult Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}
