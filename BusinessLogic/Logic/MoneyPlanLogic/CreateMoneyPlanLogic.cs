using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Infrastructure.PgModels;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class CreateMoneyPlanLogic : ILogic<CreateMoneyPlanParam, CreateMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUserRepository _userRepository;
        private readonly IPgUsageRepository _usageRepository;
        public CreateMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository, IPgUserRepository userRepository, IPgUsageRepository usageRepository)
        {
            _moneyPlanRepository = moneyPlanRepository;
            _userRepository = userRepository;
            _usageRepository = usageRepository;
        }

        public async Task<CreateMoneyPlanResult>? Execute(CreateMoneyPlanParam param)
        {
            Log.Information($"CreateMoneyPlanLogic Param: {param}");

            var returnData = new CreateMoneyPlanResult
            {
                Result = false,
                MsgCode = "CREATE_MONEY_PLAN_FAILED",
            };

            try
            {
                var isExist = await _moneyPlanRepository.IsExistMoneyPlan(param.DateTime, param.Type, Guid.Parse(param.UserId));
                if (isExist == true)
                {
                    returnData.MsgCode = "EXIST_MONEY_PLAN";
                    return returnData;
                }

                var moneyPlanCreate = await CreateMoneyPlanFucAsync(param);

                if (moneyPlanCreate == null)
                    return returnData;

                await _moneyPlanRepository.AddParentTotalChildrenMoney(moneyPlanCreate.Id.ToString());

                await _moneyPlanRepository.CalculateTotalChildrenMoney(moneyPlanCreate.Id.ToString());

                return new CreateMoneyPlanResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    Data = new CreateMoneyPlanDataResult
                    {
                        Id = moneyPlanCreate.Id.ToString(),
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Information($"CreateMoneyPlanLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
        public async Task<PgMoneyPlan?> CreateMoneyPlanFucAsync(CreateMoneyPlanParam param)
        {
            var usageMoneys = new List<PgUsageMoney>();

            var resultData = await _moneyPlanRepository.CreateMoneyPlan(new PgMoneyPlan
            {
                UserId = Guid.Parse(param.UserId),
                Type = param.Type,
                ExpectAmount = param.ExpectAmount ?? 0,
                ActualAmount = param.ExpectAmount ?? 0,
                CurrencyUnit = param.CurrencyUnit,
                Day = param.DateTime.Day,
                Month = param.DateTime.Month,
                Year = param.DateTime.Year,

                CreatorId = Guid.Parse(param.UserId),
                CreationTime = DateTime.Now,
            });

            if (param.UsageMoneys != null)
            {
                foreach (var item in param.UsageMoneys)
                {
                    // Set Expect = Actual => isn't Done
                    var usageMoney = new PgUsageMoney
                    {
                        Id = Guid.NewGuid(),
                        MoneyPlanId = resultData?.Id,
                        Name = item.Name,
                        ExpectAmount = item.ExpectAmount ?? 0,
                        ActualAmount = item.ExpectAmount ?? 0,
                        Priority = item.Priority,
                        CategoryId = item.CategoryId.IsNullOrEmpty() ? null : Guid.Parse(item.CategoryId)
                    };
                    usageMoneys.Add(usageMoney);
                }
            }
            await _usageRepository.CreateUsages(usageMoneys, param.UserId);

            return resultData;
        }
    }
}