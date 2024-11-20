using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class UpdateUsageMoneyLogic : ILogic<UpdateUsageMoneyParam, UpdateUsageMoneyResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUserRepository _userRepository;
        private readonly IPgUsageRepository _usageRepository;
        private readonly IAutoMap _autoMap;
        public UpdateUsageMoneyLogic(IPgMoneyPlanRepository moneyPlanRepository, IPgUserRepository userRepository, IPgUsageRepository usageRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
            _autoMap = autoMap;
        }

        public async Task<UpdateUsageMoneyResult>? Execute(UpdateUsageMoneyParam param)
        {
            Log.Information($"UpdateUsageMoneyLogic Param: {param}");

            var returnData = new UpdateUsageMoneyResult
            {
                Result = false,
                MsgCode = "UPDATE_USAGE_FAILED",
            };

            try
            {
                if (param.MoneyPlanId == null || param.MoneyPlanId == "")
                {
                    returnData.MsgCode = "MONEY_PLAN_NOT_FOUND";
                    return returnData;
                }

                var moneyPlan = await _moneyPlanRepository.GetMoneyPlan(param.MoneyPlanId);

                if (moneyPlan == null)
                {
                    returnData.MsgCode = "MONEY_PLAN_NOT_FOUND";
                    return returnData;
                }

                var usageMoneys = new List<PgUsageMoney>();

                foreach (var item in param.Data)
                {
                    Guid? category = null;
                    if (!item.CategoryId.IsNullOrEmpty())
                    {
                        category = (await _userRepository.GetCategoryInUserById(item.CategoryId, param.UserId)).Id;
                    }

                    var usageMoney = new PgUsageMoney
                    {
                        Id = Guid.NewGuid(),
                        MoneyPlanId = moneyPlan.Id,
                        Name = item.Name,
                        Priority = item.Priority,
                        ActualAmount = item.ActualAmount,
                        ExpectAmount = item.ExpectAmount,
                        CategoryId = category
                    };

                    usageMoneys.Add(usageMoney);
                }

                await _usageRepository.DeleteAllUsageFromMoneyPlan(moneyPlan.Id);
                await _usageRepository.CreateUsages(usageMoneys, param.UserId);


                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateUsageMoneyDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"UpdateMoneyPlanUsageLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}