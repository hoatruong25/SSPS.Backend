using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infrastructure.Models;
using DTO.Params.UserParam;
using DTO.Results.UserResult;
using DTO.Results;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using DTO.Const;

namespace Repository.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserRepository(IOptions<SSPSDataSettings> sspsDataSettings)
        {
            var mongoClient = new MongoClient(sspsDataSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(sspsDataSettings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(sspsDataSettings.Value.UserCollectionName);
        }

        public List<CategoryUsageMoney> GenerateDefaultWorkPlan()
        {
            return new List<CategoryUsageMoney>{

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Education",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Entertainment",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Food & Beverage",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Health & Fitness",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Electricity Bill",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Gas Bill",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Internet Bill",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Phone Bill",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Water Bill",
                IsDefault = true,
                IsDelete = false,
            },

            new CategoryUsageMoney {
                _id = ObjectId.GenerateNewId(),
                Name = "Other",
                IsDefault = true,
                IsDelete = false,
            },
        };
        }

        public async Task<List<User>> GetListUser()
        {
            var resultData = (await _userCollection.FindAsync(_ => true)).ToList();
            return resultData;
        }

        public async Task<User?> GetUser(string id)
        {
            var resultData = (await _userCollection.FindAsync(x => x._id.ToString() == id && x.IsDelete == false)).FirstOrDefault();
            return resultData;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var resultData = (await _userCollection.FindAsync(x => x.Email == email && x.IsDelete == false)).FirstOrDefault();
            return resultData;
        }

        public async Task<bool> CreateUser(User user)
        {
            user.CategoryUsageMoney = GenerateDefaultWorkPlan();
            await _userCollection.InsertOneAsync(user);
            return true;
        }

        public async Task<bool> IsEmailExit(string email)
        {
            var resultData = (await _userCollection.FindAsync(x => x.Email == email && x.IsDelete == false)).FirstOrDefault();

            if (resultData == null) return false;

            return true;
        }

        public async Task<bool> IsCodeExit(string code)
        {
            var resultData = (await _userCollection.FindAsync(x => x.Code == code && x.IsDelete == false)).FirstOrDefault();

            if (resultData == null) return false;

            return true;
        }

        public async Task<GetListUserResult> GetListUser(GetListUserParam param)
        {
            var data = (await _userCollection.FindAsync(x => x.IsDelete == false)).ToList();

            var resultData = data
                .Where(x => x.IsDelete == false)
                .OrderByDescending(x => x.CreationTime)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(x => new GetListUserDataResult
                {
                    Id = x._id.ToString(),
                    Code = x.Code,
                    Email = x.Email,
                    Phone = x.Phone,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    School = x.School,
                    Location = x.Location,
                    Status = x.Status,
                }).ToList();



            var totalUser = data.Count();

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

        public async Task<User> UpdateUser(User user)
        {
            var resultData = await _userCollection.ReplaceOneAsync(x => x._id == user._id, user);

            return _userCollection.Find(x => x._id == user._id).FirstOrDefault();
        }

        public async Task<CategoryUsageMoney?> GetCategoryInUserById(string categoryId, string userId)
        {
            var user = await _userCollection.AsQueryable().FirstOrDefaultAsync(x => x._id.ToString() == userId);

            if (user != null)
            {
                var categoryUsageMoney = user.CategoryUsageMoney?.Find(c => c._id.ToString() == categoryId);
                return categoryUsageMoney;
            }

            return null;
        }

        public async Task<List<CategoryUsageMoney>> GetListCategoryByUserId(string userId)
        {
            var resultData = await _userCollection.Find(x => x._id == ObjectId.Parse(userId) && x.IsDelete == false).FirstOrDefaultAsync();

            if (resultData?.CategoryUsageMoney == null)
                return new List<CategoryUsageMoney>();

            return resultData.CategoryUsageMoney;
        }

        public async Task<List<int>> GetTotalUserEachMonth(int year)
        {
            var users = (await _userCollection.FindAsync(x => x.IsDelete == false && x.Role != UserConst.USER_ROLE_ADMIN && x.CreationTime.Value.Year == year)).ToList();
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

        public async Task<int> GetTotalUser()
        {
            return await _userCollection.AsQueryable().Where(x => x.IsDelete != true && x.Role != UserConst.USER_ROLE_ADMIN).CountAsync();
        }
    }
}