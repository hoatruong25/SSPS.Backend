using DTO.Const;
using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgMoneyPlanRepo
{
    public class PgMoneyPlanRepository : IPgMoneyPlanRepository
    {
        private readonly PgDbContext _pgDbContext;
        public PgMoneyPlanRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        public async Task AddParentTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _pgDbContext.MoneyPlans.Where(x => x.Id == Guid.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan == null) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                // add total children money in year
                if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
                {
                    var parentYear = await _pgDbContext.MoneyPlans.Where(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR && x.IsDelete != true).FirstOrDefaultAsync();

                    if (parentYear != null)
                    {
                        parentYear.TotalChildrenMoney += moneyPlan.ActualAmount;
                        _pgDbContext.Update(parentYear);
                        await _pgDbContext.SaveChangesAsync();
                    }
                }
            }

            // add total children money in month
            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                var parentMonth = await _pgDbContext.MoneyPlans.Where(x => x.Month == moneyPlan.Month && x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).FirstOrDefaultAsync();

                if (parentMonth != null)
                {
                    parentMonth.TotalChildrenMoney += moneyPlan.ActualAmount;
                    _pgDbContext.Update(parentMonth);

                    await _pgDbContext.SaveChangesAsync();
                }
            }
        }

        public async Task CalculateTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _pgDbContext.MoneyPlans.Where(x => x.Id == Guid.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan?.Id == null) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
            {
                var totalChildrenMoney = await _pgDbContext.MoneyPlans.Where(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).SumAsync(x => x.ActualAmount);

                moneyPlan.TotalChildrenMoney = totalChildrenMoney ?? 0;
                _pgDbContext.Update(moneyPlan);

                await _pgDbContext.SaveChangesAsync();
                return;
            }

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                var totalChildrenMoney = await _pgDbContext.MoneyPlans.Where(x => x.Year == moneyPlan.Year && x.Month == moneyPlan.Month && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY && x.IsDelete != true).SumAsync(x => x.ActualAmount);

                moneyPlan.TotalChildrenMoney = totalChildrenMoney ?? 0;
                _pgDbContext.Update(moneyPlan);

                await _pgDbContext.SaveChangesAsync();
                return;
            }
        }

        public async Task<PgMoneyPlan?> CreateMoneyPlan(PgMoneyPlan moneyPlan)
        {
            var result = _pgDbContext.MoneyPlans.Add(moneyPlan);
            await _pgDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<PgMoneyPlan>> GetListMoneyPlanByDateRange(string userId, string fromDate, string toDate)
        {
            var startDate = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);
            var result = await _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && startDate <= x.Date && endDate >= x.Date && x.IsDelete != true).ToListAsync();

            return result;
        }

        public async Task<bool> CreateListMoneyPlan(List<PgMoneyPlan> moneyPlans)
        {
            _pgDbContext.MoneyPlans.AddRange(moneyPlans);
            await _pgDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<PgMoneyPlan>> GetListMoneyPlanInRange(string userId, string type, string fromDate, string toDate)
        {
            var startDate = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);
            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                //var resultData = await _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && x.Type == type && x.Day >= startDate.Day && x.Day <= endDate.Day && x.Month >= startDate.Month && x.Month <= endDate.Month && x.Year >= startDate.Year && x.Year <= endDate.Year && x.IsDelete != true).ToListAsync();

                var resultData = await _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && x.Type == type && new DateTime(x.Year.Value, x.Month.Value, x.Day.Value) >= startDate && new DateTime(x.Year.Value, x.Month.Value, x.Day.Value) <= endDate && x.IsDelete != true).ToListAsync();

                return resultData;
            }

            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                var resultData = await _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && new DateTime(x.Year.Value, x.Month.Value, 1) >= startDate && new DateTime(x.Year.Value, x.Month.Value + 1, 1) <= endDate && x.IsDelete != true).ToListAsync();

                return resultData;
            }

            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
            {
                var resultData = await _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && x.Type == type && x.Year >= startDate.Year && x.Year <= endDate.Year && x.IsDelete != true).ToListAsync();

                return resultData;
            }

            return new List<PgMoneyPlan>();
        }

        public async Task<PgMoneyPlan?> GetMoneyPlan(string id)
        {
            var resultData = await _pgDbContext.MoneyPlans.Where(x => x.Id == Guid.Parse(id) && x.IsDelete == false).FirstOrDefaultAsync();
            return resultData;
        }

        public async Task<PgMoneyPlan?> GetMoneyPlanByDateTime(int? day, int? month, int? year)
        {
            var resultData = new PgMoneyPlan();

            var query = _pgDbContext.MoneyPlans.Where(x => x.IsDelete == false);

            var type = "";

            if (year != null)
            {
                type = MoneyPlanConst.MONEY_PLAN_TYPE_YEAR;
                query = query.Where(x => x.Year == year);
            }

            if (month != null)
            {
                type = MoneyPlanConst.MONEY_PLAN_TYPE_MONTH;
                query = query.Where(x => x.Month == month);
            }

            if (day != null)
            {
                type = MoneyPlanConst.MONEY_PLAN_TYPE_DAY;
                query = query.Where(x => x.Day == day);
            }

            query = query.Where(x => x.Type == type);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetTotalMoneyPlanEachMonth(int year)
        {
            var moneyPlans = await _pgDbContext.MoneyPlans.Where(x => x.IsDelete == false && x.CreationTime.Value.Year == year).ToListAsync();
            var resultData = new List<int>
            {
                moneyPlans.Where(x => x.CreationTime.Value.Month == 1).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 2).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 3).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 4).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 5).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 6).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 7).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 8).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 9).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 10).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 11).Count(),
                moneyPlans.Where(x => x.CreationTime.Value.Month == 12).Count(),
            };

            return resultData;
        }

        public async Task<bool> IsExistMoneyPlan(DateTime moneyPlanDate, string Type, Guid userId)
        {
            if (Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
                return await _pgDbContext.MoneyPlans.Where(x => x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true && x.UserId == userId).AnyAsync();

            if (Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
                return await _pgDbContext.MoneyPlans.Where(x => x.Month == moneyPlanDate.Month && x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true && x.UserId == userId).AnyAsync();

            return await _pgDbContext.MoneyPlans.Where(x => x.Day == moneyPlanDate.Day && x.Month == moneyPlanDate.Month && x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true && x.UserId == userId).AnyAsync();
        }

        public async Task SubtractParentTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _pgDbContext.MoneyPlans.Where(x => x.Id == Guid.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan == null) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                // Get parent year
                var parentYear = await _pgDbContext.MoneyPlans.Where(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR && x.IsDelete != true).FirstOrDefaultAsync();

                // subtract total children money in year
                if (parentYear != null)
                {
                    parentYear.TotalChildrenMoney -= moneyPlan.ActualAmount;
                    _pgDbContext.Update(parentYear);
                    await _pgDbContext.SaveChangesAsync();
                }
            }

            // subtract total children money in month
            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                var parentMonth = await _pgDbContext.MoneyPlans.Where(x => x.Month == moneyPlan.Month && x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).FirstOrDefaultAsync();

                if (parentMonth != null)
                {
                    parentMonth.TotalChildrenMoney -= moneyPlan.ActualAmount;
                    _pgDbContext.Update(parentMonth);
                    await _pgDbContext.SaveChangesAsync();
                }
            }

            return;
        }

        public async Task<PgMoneyPlan?> UpdateMoneyPlan(PgMoneyPlan moneyPlan)
        {
            var resultData = _pgDbContext.Update(moneyPlan);
            await _pgDbContext.SaveChangesAsync();

            return resultData.Entity;
        }

        public async Task<bool> UpdateUsageMoney(List<PgUsageMoney> usageMoneys)
        {
            _pgDbContext.UpdateRange(usageMoneys);

            await _pgDbContext.SaveChangesAsync();
            return true;
        }
    }
}