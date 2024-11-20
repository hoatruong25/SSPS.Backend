using DTO.Const;
using DTO.Params.UserParam;
using DTO.Results;
using DTO.Results.UserResult;
using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgUserRepo
{
    public class PgUserRepository : IPgUserRepository
    {
        private readonly PgDbContext _pgDbContext;

        public PgUserRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }
        public async Task CreateDefaultCategoryInUser(Guid userId)
        {
            var listCategory = new List<PgCategoryUsageMoney>{
                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Education",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Entertainment",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Food & Beverage",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Health & Fitness",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Electricity Bill",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Gas Bill",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Internet Bill",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Phone Bill",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Water Bill",
                    IsDefault = true,
                    IsDelete = false,
                },

                new PgCategoryUsageMoney {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Other",
                    IsDefault = true,
                    IsDelete = false,
                },
            };
            await _pgDbContext.Categories.AddRangeAsync(listCategory);
        }
        public async Task<bool> CreateUser(PgUser user)
        {
            var result = _pgDbContext.Add(user);
            await CreateDefaultCategoryInUser(result.Entity.Id);
            await _pgDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PgCategoryUsageMoney?> GetCategoryInUserById(string categoryId, string userId)
        {
            var result = await _pgDbContext.Categories.Where(x => x.Id == Guid.Parse(categoryId) && x.UserId == Guid.Parse(userId)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<PgCategoryUsageMoney>> GetListCategoryByUserId(string userId)
        {
            var resultData = await _pgDbContext.Categories.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();

            return resultData;
        }

        public async Task<List<PgUser>> GetListUser()
        {
            var resultData = await _pgDbContext.Users.Where(x => x.IsDelete != true).ToListAsync();
            return resultData;
        }

        // Get List user for admin get all status
        public async Task<GetListUserResult> GetListUser(GetListUserParam param)
        {
            var resultData = await _pgDbContext.Users
                .Where(x => x.IsDelete == false)
                .OrderByDescending(x => x.CreationTime)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(x => new GetListUserDataResult
                {
                    Id = x.Id.ToString(),
                    Code = x.Code,
                    Email = x.Email,
                    Phone = x.Phone,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    School = x.School,
                    Location = x.Location,
                    Status = x.Status,
                }).ToListAsync();



            var totalUser = await _pgDbContext.Users.Where(x => x.IsDelete != true).CountAsync();

            return new GetListUserResult
            {
                Data = resultData,
                PaginationResult = new PaginationResult
                {
                    Page = param.Page,
                    PageSize = param.PageSize,
                    Total = totalUser,
                    TotalPage = (int)Math.Ceiling((double)totalUser / param.PageSize)
                }
            };
        }

        public async Task<int> GetTotalUser()
        {
            return await _pgDbContext.Users.Where(x => x.IsDelete != true && x.Status == UserConst.USER_STATUS_ACTIVE && x.Role != UserConst.USER_ROLE_ADMIN).CountAsync();
        }

        public async Task<List<int>> GetTotalUserEachMonth(int year)
        {
            var users = await _pgDbContext.Users.Where(x => x.IsDelete == false && x.Status == UserConst.USER_STATUS_ACTIVE && x.Role != UserConst.USER_ROLE_ADMIN && x.CreationTime.Value.Year == year).ToListAsync();
            var resultData = new List<int>
            {
                users.Where(x => x.CreationTime.Value.Month == 1).Count(),
                users.Where(x => x.CreationTime.Value.Month == 2).Count(),
                users.Where(x => x.CreationTime.Value.Month == 3).Count(),
                users.Where(x => x.CreationTime.Value.Month == 4).Count(),
                users.Where(x => x.CreationTime.Value.Month == 5).Count(),
                users.Where(x => x.CreationTime.Value.Month == 6).Count(),
                users.Where(x => x.CreationTime.Value.Month == 7).Count(),
                users.Where(x => x.CreationTime.Value.Month == 8).Count(),
                users.Where(x => x.CreationTime.Value.Month == 9).Count(),
                users.Where(x => x.CreationTime.Value.Month == 10).Count(),
                users.Where(x => x.CreationTime.Value.Month == 11).Count(),
                users.Where(x => x.CreationTime.Value.Month == 12).Count(),
            };

            return resultData;
        }

        public async Task<PgUser?> GetUser(string id)
        {
            var resultData = await _pgDbContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.Status == UserConst.USER_STATUS_ACTIVE && x.IsDelete == false);
            return resultData;
        }

        public async Task<PgUser?> GetUserByEmail(string email)
        {
            var resultData = await _pgDbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDelete == false && x.Status == UserConst.USER_STATUS_ACTIVE);
            return resultData;
        }

        public async Task<PgUser?> GetUserByEmailToActive(string email)
        {
            var resultData = await _pgDbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDelete == false && x.Status == UserConst.USER_STATUS_BLOCK);
            return resultData;
        }

        public async Task<bool> IsCodeExit(string code)
        {
            var resultData = await _pgDbContext.Users.FirstOrDefaultAsync(x => x.Code == code && x.IsDelete == false && x.Status == UserConst.USER_STATUS_ACTIVE);

            if (resultData == null) return false;

            return true;
        }

        public async Task<bool> IsEmailExit(string email)
        {
            var resultData = await _pgDbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDelete == false && x.Status == UserConst.USER_STATUS_ACTIVE);

            if (resultData == null) return false;

            return true;
        }

        public async Task<PgUser> UpdateUser(PgUser user)
        {
            var resultData = _pgDbContext.Update(user);
            await _pgDbContext.SaveChangesAsync();

            return resultData.Entity;
        }

        public DashboardUserDataResult GetDashboard(string userId, string type, DateTime fromDate, DateTime toDate)
        {
            var result = new DashboardUserDataResult
            {
                TotalActualMoney = 0,
                TotalExpectMoney = 0,
                ListDiagramData = new List<DashboardUserDiagramDataResult>()
            };

            if (type.ToUpper() == "MONTH")
            {
                // Anh Bảo cần truyền giờ 00-00-00 vào
                var startDate = fromDate.AddSeconds(-1);
                var endDate = toDate.AddDays(1).AddSeconds(-1);

                var moneyPlans = _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && x.Date.Value >= startDate && x.Date.Value <= endDate && x.IsDelete != true).ToList();

                result.TotalExpectMoney = moneyPlans.Sum(x => x.ExpectAmount);
                result.TotalActualMoney = moneyPlans.Sum(x => x.ActualAmount.Value);

                result.ListDiagramData = moneyPlans.Select(x => new DashboardUserDiagramDataResult
                {
                    ActualMoney = x.ActualAmount.Value,
                    ExpectMoney = x.ExpectAmount,
                    DoM = x.Date.Value.Day
                }).ToList();

            }
            else
            {
                var moneyPlans = _pgDbContext.MoneyPlans.Where(x => x.UserId == Guid.Parse(userId) && x.Date.Value.Year == fromDate.Year && x.IsDelete != true).ToList();

                result.TotalExpectMoney = moneyPlans.Sum(x => x.ExpectAmount);
                result.TotalActualMoney = moneyPlans.Sum(x => x.ActualAmount.Value);

                for (int i = 1; i <= 12; i++)
                {
                    result.ListDiagramData.Add(new DashboardUserDiagramDataResult
                    {
                        ActualMoney = moneyPlans.Where(x => x.Date.Value.Month == i).Sum(x => x.ActualAmount.Value),
                        ExpectMoney = moneyPlans.Where(x => x.Date.Value.Month == i).Sum(x => x.ExpectAmount),
                        DoM = i
                    });
                }
            }

            return result;
        }
    }
}