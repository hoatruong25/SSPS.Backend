using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgUsageRepo
{
    public interface IPgUsageRepository
    {
        Task CreateUsages(List<PgUsageMoney> usages, string userId);
        Task DeleteAllUsageFromMoneyPlan(Guid moneyPlanId);
        Task<List<PgUsageMoney>> GetListUsageMoneyByMoneyPlanId(Guid moneyPlanId);
    }
}
