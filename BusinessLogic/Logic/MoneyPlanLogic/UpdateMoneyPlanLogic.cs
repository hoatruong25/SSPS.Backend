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
    public class UpdateMoneyPlanLogic : ILogic<UpdateMoneyPlanParam, UpdateMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUserRepository _userRepository;
        private readonly IPgUsageRepository _usageRepository;
        private readonly IAutoMap _autoMap;
        public UpdateMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository, IPgUserRepository userRepository, IPgUsageRepository usageRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
            _autoMap = autoMap;
        }

        public async Task<UpdateMoneyPlanResult>? Execute(UpdateMoneyPlanParam param)
        {
            Log.Information($"UpdateMoneyLogic Param: {param}");

            var returnData = new UpdateMoneyPlanResult
            {
                Result = false,
                MsgCode = "UPDATE_MONEY_PLAN_FAILED",
            };

            try
            {
                var moneyPlan = await _moneyPlanRepository.GetMoneyPlan(param.Id);

                if (moneyPlan == null)
                {
                    returnData.MsgCode = "MONEY_PLAN_NOT_FOUND";
                    return returnData;
                }

                // subtract parent TotalChildrenMoney
                // await _moneyPlanRepository.SubtractParentTotalChildrenMoney(param.Id);

                // Update Money Plan
                await UpdateMoneyPlanAsync(moneyPlan, param);

                // add parent TotalChildrenMoney
                // await _moneyPlanRepository.AddParentTotalChildrenMoney(param.Id);

                // add current TotalChildrenMoney
                // await _moneyPlanRepository.CalculateTotalChildrenMoney(param.Id);

                return new UpdateMoneyPlanResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    Data = new UpdateMoneyPlanDataResult(),
                };
            }
            catch (Exception ex)
            {
                Log.Information($"UpdateMoneyPlanUsageLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }

        public async Task UpdateMoneyPlanAsync(PgMoneyPlan entity, UpdateMoneyPlanParam param)
        {
            entity.Status = param.Status;
            entity.ExpectAmount = param.ExpectAmount;
            entity.ActualAmount = param.ActualAmount;
            entity.Day = param.Day;
            entity.Month = param.Month;
            entity.Year = param.Year;
            entity.LastModificationTime = DateTime.Now;
            var usageMoneys = new List<PgUsageMoney>();
            foreach (var item in param.Usages)
            {
                Guid? category = null;
                if (!item.CategoryId.IsNullOrEmpty())
                {
                    category = (await _userRepository.GetCategoryInUserById(item.CategoryId, param.UserId)).Id;
                }
                var usageMoney = new PgUsageMoney
                {
                    Id = Guid.NewGuid(),
                    MoneyPlanId = entity.Id,
                    Name = item.Name,
                    ExpectAmount = item.ExpectAmount,
                    ActualAmount = item.ActualAmount,
                    Priority = item.Priority,
                    CategoryId = category,
                };

                usageMoneys.Add(usageMoney);
            }

            await _usageRepository.DeleteAllUsageFromMoneyPlan(entity.Id);
            await _usageRepository.CreateUsages(usageMoneys, param.UserId);
            await _moneyPlanRepository.UpdateMoneyPlan(entity);
        }
    }
}