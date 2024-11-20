using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Infrastructure.Models;

namespace Repository.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<List<User>> GetListUser();
        Task<User?> GetUser(string id);
        Task<User?> GetUserByEmail(string email);
        Task<bool> IsEmailExit(string email);
        Task<bool> IsCodeExit(string code);
        Task<bool> CreateUser(User user);
        Task<GetListUserResult> GetListUser(GetListUserParam param);
        Task<User> UpdateUser(User user);
        Task<CategoryUsageMoney?> GetCategoryInUserById(string categoryId, string userId);
        Task<List<CategoryUsageMoney>> GetListCategoryByUserId(string userId);
        Task<List<int>> GetTotalUserEachMonth(int year);
        Task<int> GetTotalUser();
    }
}