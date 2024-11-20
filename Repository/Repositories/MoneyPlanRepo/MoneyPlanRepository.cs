using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infrastructure.Models;
using MongoDB.Bson;
using DTO.Const;

namespace Repository.Repositories.MoneyPlanRepo
{
    public class MoneyPlanRepository : IMoneyPlanRepository
    {
        private readonly IMongoCollection<MoneyPlan> _moneyPlanCollection;
        public MoneyPlanRepository(IOptions<SSPSDataSettings> sspsDataSettings)
        {
            var mongoClient = new MongoClient(sspsDataSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(sspsDataSettings.Value.DatabaseName);
            _moneyPlanCollection = mongoDatabase.GetCollection<MoneyPlan>(sspsDataSettings.Value.MoneyPlanCollectionName);
        }

        public async Task<MoneyPlan?> CreateMoneyPlan(MoneyPlan moneyPlan)
        {
            var Id = ObjectId.GenerateNewId();
            moneyPlan._id = Id;

            await _moneyPlanCollection.InsertOneAsync(moneyPlan);

            return (await _moneyPlanCollection.FindAsync(x => x._id == Id)).FirstOrDefault();
        }

        public async Task<MoneyPlan?> GetMoneyPlanByDateTime(int? day, int? month, int? year)
        {
            var resultData = new MoneyPlan();

            var query = _moneyPlanCollection.AsQueryable().Where(x => x.IsDelete == false);

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

            return query.FirstOrDefault();
        }

        public async Task<MoneyPlan?> GetMoneyPlan(string id)
        {
            var resultData = (await _moneyPlanCollection.FindAsync(x => x._id.ToString() == id && x.IsDelete == false)).FirstOrDefault();
            return resultData;
        }

        public async Task<MoneyPlan?> UpdateMoneyPlan(MoneyPlan moneyPlan)
        {
            var resultData = await _moneyPlanCollection.ReplaceOneAsync(x => x._id == moneyPlan._id, moneyPlan);

            return _moneyPlanCollection.Find(x => x._id == moneyPlan._id).FirstOrDefault();
        }

        public async Task<List<MoneyPlan>> GetListMoneyPlanInRange(string userId, string type, string fromDate, string toDate)
        {
            var startDate = DateTime.Parse(fromDate);
            var endDate = DateTime.Parse(toDate);
            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                var resultData = (await _moneyPlanCollection.FindAsync(x => x.UserId == ObjectId.Parse(userId) && x.Type == type && x.Day >= startDate.Day && x.Day <= endDate.Day && x.Month >= startDate.Month && x.Month <= endDate.Month && x.Year >= startDate.Year && x.Year <= endDate.Year && x.IsDelete != true)).ToList();

                return resultData;
            }

            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                var resultData = (await _moneyPlanCollection.FindAsync(x => x.UserId == ObjectId.Parse(userId) && x.Type == type && x.Month >= startDate.Month && x.Month <= endDate.Month && x.Year >= startDate.Year && x.Year <= endDate.Year && x.IsDelete != true)).ToList();

                return resultData;
            }

            if (type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
            {
                var resultData = (await _moneyPlanCollection.FindAsync(x => x.UserId == ObjectId.Parse(userId) && x.Type == type && x.Year >= startDate.Year && x.Year <= endDate.Year && x.IsDelete != true)).ToList();

                return resultData;
            }

            return new List<MoneyPlan>();
        }

        public async Task<bool> UpdateUsageMoney(string moneyPlanId, List<UsageMoney> usageMoneys)
        {
            var moneyPlan = await _moneyPlanCollection.Find(x => x._id == ObjectId.Parse(moneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan == null)
                return false;

            moneyPlan.UsageMoneys = usageMoneys;
            await _moneyPlanCollection.ReplaceOneAsync(x => x._id == moneyPlan._id, moneyPlan);

            return true;
        }

        public async Task SubtractParentTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _moneyPlanCollection.Find(x => x._id == ObjectId.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan == null) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                // Get parent year
                var parentYear = await _moneyPlanCollection.Find(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR && x.IsDelete != true).FirstOrDefaultAsync();

                // subtract total children money in year
                if (parentYear != null)
                {
                    parentYear.TotalChildrenMoney -= moneyPlan.ActualAmount;
                    await _moneyPlanCollection.ReplaceOneAsync(x => x._id == parentYear._id, parentYear);
                }
            }

            // subtract total children money in month
            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                var parentMonth = await _moneyPlanCollection.Find(x => x.Month == moneyPlan.Month && x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).FirstOrDefaultAsync();

                if (parentMonth != null)
                {
                    parentMonth.TotalChildrenMoney -= moneyPlan.ActualAmount;
                    await _moneyPlanCollection.ReplaceOneAsync(x => x._id == parentMonth._id, parentMonth);
                }
            }

            return;
        }

        // Just care about 1 level
        public async Task AddParentTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _moneyPlanCollection.Find(x => x._id == ObjectId.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan == null) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR) return;

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                // add total children money in year
                if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
                {
                    var parentYear = await _moneyPlanCollection.Find(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR && x.IsDelete != true).FirstOrDefaultAsync();

                    if (parentYear != null)
                    {
                        parentYear.TotalChildrenMoney += moneyPlan.ActualAmount;
                        await _moneyPlanCollection.ReplaceOneAsync(x => x._id == parentYear._id, parentYear);
                    }
                }
            }

            // add total children money in month
            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY)
            {
                var parentMonth = await _moneyPlanCollection.Find(x => x.Month == moneyPlan.Month && x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).FirstOrDefaultAsync();

                if (parentMonth != null)
                {
                    parentMonth.TotalChildrenMoney += moneyPlan.ActualAmount;
                    await _moneyPlanCollection.ReplaceOneAsync(x => x._id == parentMonth._id, parentMonth);
                }
            }
        }

        public async Task<bool> IsExistMoneyPlan(DateTime moneyPlanDate, string Type)
        {
            if (Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
                return await _moneyPlanCollection.Find(x => x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true).AnyAsync();

            if (Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
                return await _moneyPlanCollection.Find(x => x.Month == moneyPlanDate.Month && x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true).AnyAsync();

            return await _moneyPlanCollection.Find(x => x.Day == moneyPlanDate.Day && x.Month == moneyPlanDate.Month && x.Year == moneyPlanDate.Year && x.Type == Type && x.IsDelete != true).AnyAsync();
        }

        // Only care about child 1 level
        public async Task CalculateTotalChildrenMoney(string currentMoneyPlanId)
        {
            var moneyPlan = await _moneyPlanCollection.Find(x => x._id == ObjectId.Parse(currentMoneyPlanId) && x.IsDelete != true).FirstOrDefaultAsync();

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_YEAR)
            {
                var totalChildrenMoney = _moneyPlanCollection.AsQueryable().Where(x => x.Year == moneyPlan.Year && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH && x.IsDelete != true).Sum(x => x.ActualAmount);

                moneyPlan.TotalChildrenMoney = totalChildrenMoney ?? 0;
                await _moneyPlanCollection.ReplaceOneAsync(x => x._id == moneyPlan._id, moneyPlan);

                return;
            }

            if (moneyPlan.Type == MoneyPlanConst.MONEY_PLAN_TYPE_MONTH)
            {
                var totalChildrenMoney = _moneyPlanCollection.AsQueryable().Where(x => x.Year == moneyPlan.Year && x.Month == moneyPlan.Month && x.Type == MoneyPlanConst.MONEY_PLAN_TYPE_DAY && x.IsDelete != true).Sum(x => x.ActualAmount);

                moneyPlan.TotalChildrenMoney = totalChildrenMoney ?? 0;
                await _moneyPlanCollection.ReplaceOneAsync(x => x._id == moneyPlan._id, moneyPlan);

                return;
            }
        }

        public async Task<List<int>> GetTotalMoneyPlanEachMonth(int year)
        {
            var moneyPlans = (await _moneyPlanCollection.FindAsync(x => x.IsDelete == false && x.CreationTime.Value.Year == year)).ToList();
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
    }
}