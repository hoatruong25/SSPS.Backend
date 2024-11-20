using Infrastructure.Models;

namespace Repository.Repositories.MoneyPlanRepo
{
    public interface IMoneyPlanRepository
    {
        Task<MoneyPlan?> CreateMoneyPlan(MoneyPlan moneyPlan);

        Task<MoneyPlan?> GetMoneyPlanByDateTime(int? day, int? month, int? year);
        Task<List<MoneyPlan>> GetListMoneyPlanInRange(string userId, string type, string fromDate, string toDate);
        Task<MoneyPlan?> GetMoneyPlan(string id);

        Task<MoneyPlan?> UpdateMoneyPlan(MoneyPlan moneyPlan);
        Task<bool> UpdateUsageMoney(string moneyPlanId, List<UsageMoney> usageMoneys);
        Task SubtractParentTotalChildrenMoney(string currentMoneyPlanId);
        Task AddParentTotalChildrenMoney(string currentMoneyPlanId);
        Task<bool> IsExistMoneyPlan(DateTime moneyPlanDate, string Type);
        Task CalculateTotalChildrenMoney(string currentMoneyPlanId);
        Task<List<int>> GetTotalMoneyPlanEachMonth(int year);
    }
}