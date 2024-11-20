using DTO.Const;
using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgUsageRepo
{
    public class PgUsageRepository : IPgUsageRepository
    {
        private readonly PgDbContext _dbContext;
        public PgUsageRepository(PgDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateUsages(List<PgUsageMoney> usages, string userId)
        {
            // Add data to chat box table for LLMs
            var listChatBoxData = new List<PgChatBoxData>();

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId) && x.IsDelete != true && x.Status == UserConst.USER_STATUS_ACTIVE);

            var listCategory = await _dbContext.Categories.Where(x => x.UserId == user.Id).ToListAsync();

            foreach (var usage in usages)
            {
                var category = listCategory.FirstOrDefault(x => x.Id == usage.CategoryId);

                listChatBoxData.Add(new PgChatBoxData
                {
                    UserId = user.Id,
                    Username = user.FirstName + " " + user.LastName,
                    Category = category?.Name ?? "",
                    MoneyPlanId = usage.MoneyPlanId ?? Guid.Empty,
                    Amount = usage.ActualAmount != 0 ? usage.ActualAmount : usage.ExpectAmount,
                    Date = DateTime.Now.Date
                });
            }

            _dbContext.ChatBoxData.AddRange(listChatBoxData);
            _dbContext.UsageMoneys.AddRange(usages);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllUsageFromMoneyPlan(Guid moneyPlanId)
        {
            _dbContext.RemoveRange(_dbContext.ChatBoxData.Where(x => x.MoneyPlanId == moneyPlanId));

            var usages = await _dbContext.UsageMoneys.Where(x => x.MoneyPlanId == moneyPlanId).ToListAsync();
            _dbContext.RemoveRange(usages);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PgUsageMoney>> GetListUsageMoneyByMoneyPlanId(Guid moneyPlanId)
        {
            var usages = await _dbContext.UsageMoneys.Where(x => x.MoneyPlanId == moneyPlanId).ToListAsync();
            return usages;
        }
    }
}
