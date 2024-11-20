using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Serilog;

namespace BusinessLogic.Logic.MoneyPlanLogic
{
    public class CreateListMoneyPlanLogic : ILogic<CreateListMoneyPlanParam, CreateListMoneyPlanResult>
    {
        private readonly IPgMoneyPlanRepository _moneyPlanRepository;
        private readonly IPgUsageRepository _usageRepository;
        public CreateListMoneyPlanLogic(IPgMoneyPlanRepository moneyPlanRepository, IPgUsageRepository usageRepository)
        {
            _moneyPlanRepository = moneyPlanRepository;
            _usageRepository = usageRepository;
        }
        public async Task<CreateListMoneyPlanResult>? Execute(CreateListMoneyPlanParam param)
        {
            Log.Information($"CreateListMoneyPlanLogic Param: {param}");

            var returnData = new CreateListMoneyPlanResult
            {
                Result = false,
                MsgCode = "CREATE_LIST_MONEY_PLAN_FAILED",
            };

            try
            {
                var fromDate = DateTime.Parse(param.FromDate).Date;
                var toDate = DateTime.Parse(param.ToDate).Date;

                if (toDate < fromDate)
                {
                    returnData.MsgCode = "FROM_DATE_TO_DATE_INVALID";
                    return returnData;
                }


                var listMoneyPlanInRangeDate = await _moneyPlanRepository.GetListMoneyPlanByDateRange(param.UserId, fromDate.ToString(), toDate.ToString());
                var totalDateRange = (toDate - fromDate).TotalDays + 1;
                var totalDateActual = totalDateRange - listMoneyPlanInRangeDate.Count();
                var totalMoney = CalculateTotalMoneyInRange(listMoneyPlanInRangeDate);

                if (totalDateActual == listMoneyPlanInRangeDate.Count)
                {
                    returnData.MsgCode = "PLAN_IS_EXISTING";
                    return returnData;
                }

                // Kiểm tra số tiền truyền vào lớn hơn số tiền đã được tạo hay chưa
                if (totalMoney >= param.ExpectAmount)
                {
                    returnData.MsgCode = "EXPECT_AMOUNT_IS_NOT_ENOUGH";
                    return returnData;
                }

                // Kiểm tra số tiền của usage có lớn hơn số tiền truyền vào - số tiền được tạo
                if (param.UsageMoneys != null)
                {
                    var totalUsageMoney = param.UsageMoneys.Sum(x => x.ExpectAmount);

                    if (totalUsageMoney > param.ExpectAmount - totalMoney)
                    {
                        returnData.MsgCode = "TOTAL_USAGE_MONEY_IS_TOO_LARGE";
                        return returnData;
                    }
                }


                var listUsage = new List<PgUsageMoney>();
                if (param.UsageMoneys != null)
                {
                    foreach (var item in param.UsageMoneys)
                    {
                        listUsage.Add(new PgUsageMoney
                        {
                            Name = item.Name,
                            CategoryId = Guid.Parse(item.CategoryId),
                            ExpectAmount = Math.Round(item.ExpectAmount.Value / totalDateActual, 2),
                            ActualAmount = 0,
                            Priority = item.Priority,
                        });

                    }
                }

                // Date đang bị sai phải chọn những date ở giữa những ngày đã tạo rồi
                for (int i = 0; i < totalDateRange; i++)
                {
                    if (listMoneyPlanInRangeDate.Where(x => x.Date == fromDate.AddDays(i)).Any())
                        continue;
                    var moneyPlanCreate = await _moneyPlanRepository.CreateMoneyPlan(new PgMoneyPlan
                    {
                        UserId = Guid.Parse(param.UserId),
                        ExpectAmount = (param.ExpectAmount - totalMoney) / totalDateRange,
                        ActualAmount = 0,
                        CurrencyUnit = param.CurrencyUnit,
                        CreationTime = DateTime.Now,
                        CreatorId = Guid.Parse(param.UserId),
                        Date = fromDate.AddDays(i),
                        IsDelete = false,
                        Type = "",
                    });

                    if (moneyPlanCreate == null)
                    {
                        returnData.MsgCode = "CREATE_MONEY_PLAN_FAILED";
                        return returnData;
                    }

                    await CreateUsage(listUsage, moneyPlanCreate.Id, param.UserId);
                }

                returnData.Data = new CreateListMoneyPlanDataResult();
                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"CreateListMoneyPlanLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }

        public double CalculateTotalMoneyInRange(List<PgMoneyPlan> moneyPlans)
        {
            double total = 0;
            foreach (var item in moneyPlans)
            {
                if (item.ActualAmount == 0 || item.ActualAmount == null)
                {
                    total += item.ExpectAmount;
                }
                else
                {
                    total += item.ActualAmount.Value;
                }
            }

            return total;
        }

        public async Task CreateUsage(List<PgUsageMoney> usageMoneys, Guid moneyPlanId, string userId)
        {
            usageMoneys.ForEach(x =>
            {
                x.MoneyPlanId = moneyPlanId;
                x.Id = Guid.NewGuid();
            });
            await _usageRepository.CreateUsages(usageMoneys, userId);
        }
    }
}