using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgCategoryRepo;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class GetMoneyPlanLogic : ILogic<GetMoneyPlanParam, GetMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUsageRepository _usageRepository;
        private readonly IPgCategoryRepository _categoryRepository;
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        public GetMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository,
                                IPgUsageRepository usageRepository,
                                IPgCategoryRepository categoryRepository,
                                IPgUserRepository userRepository,
                                IAutoMap autoMap)
        {
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _autoMap = autoMap;
        }

        public async Task<GetMoneyPlanResult>? Execute(GetMoneyPlanParam param)
        {
            Log.Information($"GetMoneyPlanLogic Param: {param}");

            var returnData = new GetMoneyPlanResult
            {
                Result = false,
                MsgCode = "GET_MONEY_PLAN_FAILED",
            };

            try
            {
                var resultData = await _moneyPlanRepository.GetMoneyPlan(param.Id);

                if (resultData == null)
                {
                    returnData.MsgCode = "MONEY_PLAN_NOT_FOUND";
                    return returnData;
                }

                returnData.Data = _autoMap.Map<PgMoneyPlan, GetMoneyPlanDataResult>(resultData);
                returnData.Data.UsageMoneys = new List<GetMoneyPlanDataUsageMoneyResult>();

                var usages = await _usageRepository.GetListUsageMoneyByMoneyPlanId(resultData.Id);

                var listCategory = await _userRepository.GetListCategoryByUserId(resultData.UserId.ToString());
                foreach(var item in usages) {
                    returnData.Data.UsageMoneys.Add(new GetMoneyPlanDataUsageMoneyResult{
                        Name = item.Name,
                        ActualAmount = item.ActualAmount,
                        ExpectAmount = item.ExpectAmount,
                        Priority = item.Priority.Value,
                        CategoryName = item.CategoryId == null ? null : listCategory.Where(x => x.Id == item.CategoryId).FirstOrDefault()?.Name
                    });
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetMoneyPlanLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}