using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class GetListMoneyPlanLogic : ILogic<GetListMoneyPlanParam, GetListMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUsageRepository _usageRepository;
        private readonly IPgUserRepository _userRepository;

        private readonly IAutoMap _autoMap;
        public GetListMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository,
                                    IPgUsageRepository usageRepository,
                                    IPgUserRepository userRepository,
                                    IAutoMap autoMap)
        {
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
            _userRepository = userRepository;
            _autoMap = autoMap;
        }

        public async Task<GetListMoneyPlanResult>? Execute(GetListMoneyPlanParam param)
        {
            Log.Information($"GetListMoneyPlanLogic Param: {param}");

            var returnData = new GetListMoneyPlanResult
            {
                Result = false,
                MsgCode = "GET_LIST_MONEY_PLAN_FAILED",
            };

            try
            {
                // validate from date and to date
                if (DateTime.Parse(param.FromDate) > DateTime.Parse(param.ToDate))
                {
                    returnData.MsgDesc = "From date must be less than to date";
                    returnData.MsgCode = "FROM_DATE_TO_DATE_INVALID";
                    return returnData;
                }

                // var resultData = await _moneyPlanRepository.GetListMoneyPlanInRange(param.UserId, param.Type, param.FromDate, param.ToDate);

                var resultData = await _moneyPlanRepository.GetListMoneyPlanByDateRange(param.UserId, param.FromDate, param.ToDate);

                returnData.Data = _autoMap.Map<List<PgMoneyPlan>, List<GetListMoneyPlanDataResult>>(resultData);
                if (returnData.Data != null)
                {
                    foreach (var moneyPlan in returnData.Data)
                    {
                        moneyPlan.UsageMoneys = new List<GetListMoneyPlanDataUsageMoneyResult>();

                        var usages = await _usageRepository.GetListUsageMoneyByMoneyPlanId(Guid.Parse(moneyPlan.Id));

                        var listCategory = await _userRepository.GetListCategoryByUserId(param.UserId);
                        foreach (var item in usages)
                        {
                            moneyPlan.UsageMoneys.Add(new GetListMoneyPlanDataUsageMoneyResult
                            {
                                Name = item.Name,
                                ActualAmount = item.ActualAmount,
                                ExpectAmount = item.ExpectAmount,
                                Priority = item.Priority,
                                CategoryName = item.CategoryId == null ? null : listCategory.Where(x => x.Id == item.CategoryId).FirstOrDefault()?.Name
                            });
                        }
                    }
                }


                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetListMoneyPlanLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}