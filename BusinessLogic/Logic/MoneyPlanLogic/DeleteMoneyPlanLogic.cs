using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class DeleteMoneyPlanLogic : ILogic<DeleteMoneyPlanParam, DeleteMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUsageRepository _usageRepository;
        private readonly IAutoMap _autoMap;
        public DeleteMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository, IPgUsageRepository usageRepository, IAutoMap autoMap)
        {
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
            _autoMap = autoMap;
        }

        public async Task<DeleteMoneyPlanResult>? Execute(DeleteMoneyPlanParam param)
        {
            Log.Information($"DeleteMoneyPlanLogic Param: {param}");

            var returnData = new DeleteMoneyPlanResult
            {
                Result = false,
                MsgCode = "DELETE_MONEY_PLAN_FAILED",
            };

            try
            {
                var moneyPlan = await _moneyPlanRepository.GetMoneyPlan(param.MoneyPlanId);

                if (moneyPlan == null)
                {
                    returnData.MsgCode = "MONEY_PLAN_NOT_FOUND";
                    return returnData;
                }


                // Delete money plan
                moneyPlan.IsDelete = true;
                await _moneyPlanRepository.UpdateMoneyPlan(moneyPlan);

                // Delete usage
                await _usageRepository.DeleteAllUsageFromMoneyPlan(Guid.Parse(param.MoneyPlanId));

                return new DeleteMoneyPlanResult
                {
                    Result = true,
                    MsgCode = "SUCCESS",
                    Data = new DeleteMoneyPlanDataResult(),
                };
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