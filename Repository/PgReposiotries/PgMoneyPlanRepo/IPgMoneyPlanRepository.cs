using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgMoneyPlanRepo
{
    public interface IPgMoneyPlanRepository
    {
        Task<PgMoneyPlan?> CreateMoneyPlan(PgMoneyPlan moneyPlan);

        Task<PgMoneyPlan?> GetMoneyPlanByDateTime(int? day, int? month, int? year);
        Task<List<PgMoneyPlan>> GetListMoneyPlanInRange(string userId, string type, string fromDate, string toDate);
        Task<List<PgMoneyPlan>> GetListMoneyPlanByDateRange(string userId, string fromDate, string toDate);
        Task<bool> CreateListMoneyPlan(List<PgMoneyPlan> moneyPlans);
        Task<PgMoneyPlan?> GetMoneyPlan(string id);

        Task<PgMoneyPlan?> UpdateMoneyPlan(PgMoneyPlan moneyPlan);
        Task<bool> UpdateUsageMoney(List<PgUsageMoney> usageMoneys);
        Task SubtractParentTotalChildrenMoney(string currentMoneyPlanId);
        Task AddParentTotalChildrenMoney(string currentMoneyPlanId);
        Task<bool> IsExistMoneyPlan(DateTime moneyPlanDate, string Type, Guid userId);
        Task CalculateTotalChildrenMoney(string currentMoneyPlanId);
        Task<List<int>> GetTotalMoneyPlanEachMonth(int year);
    }
}