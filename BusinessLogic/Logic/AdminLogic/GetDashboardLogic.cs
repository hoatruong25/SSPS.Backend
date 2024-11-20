using BusinessLogic.Logic.Caching;
using DTO.Params.MoneyPlanParam;
using DTO.Results.AdminResult;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgNoteRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class GetDashboardLogic : ILogic<GetDashboardParam, GetDashboardResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgNoteRepository _noteRepository;
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly ICacheService _cacheService;
        public GetDashboardLogic(
            IPgUserRepository userRepository,
            IPgNoteRepository noteRepository,
            IPgMoneyPlanRepository moneyPlanRepository,
            ICacheService cacheService)
        {
            _userRepository = userRepository;
            _noteRepository = noteRepository;
            _moneyPlanRepository = moneyPlanRepository;
            _cacheService = cacheService;
        }

        public async Task<GetDashboardResult>? Execute(GetDashboardParam param)
        {
            Log.Information($"DashboardLogic Param: {param}");

            var returnData = new GetDashboardResult
            {
                Result = false,
                MsgCode = "GET_DASHBOARD_FAILED",
            };

            try
            {
                //GetDashboardDataResult? redisResult = await _cacheService.GetAsync<GetDashboardDataResult>(param.Year.ToString());

                //if (redisResult is not null)
                //{
                //    return new GetDashboardResult
                //    {
                //        Result = true,
                //        MsgCode = "SUCCESS",
                //        Data = redisResult
                //    };
                //}

                var amountOfUser = await _userRepository.GetTotalUser();
                var amountOfNewUser = await _userRepository.GetTotalUserEachMonth(param.Year);
                var amountOfNote = await _noteRepository.GetTotalNoteEachMonth(param.Year);
                var amountOfMoneyPlan = await _moneyPlanRepository.GetTotalMoneyPlanEachMonth(param.Year);

                var resultData = new GetDashboardDataResult
                {
                    AmountOfUser = amountOfUser,
                    AmountOfNewUser = amountOfNewUser,
                    AmountOfNote = amountOfNote,
                    AmountOfMoneyPlan = amountOfMoneyPlan,
                };

                //await _cacheService.SetAsync(param.Year.ToString(), resultData);

                return new GetDashboardResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    Data = resultData
                };
            }
            catch (Exception ex)
            {
                Log.Information($"Dashboard Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}