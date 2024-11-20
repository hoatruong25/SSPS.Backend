using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgUserRepo
{
    public interface IPgUserRepository
    {
        Task<List<PgUser>> GetListUser();
        Task<PgUser?> GetUser(string id);
        Task<PgUser?> GetUserByEmail(string email);
        Task<PgUser?> GetUserByEmailToActive(string email);
        Task<bool> IsEmailExit(string email);
        Task<bool> IsCodeExit(string code);
        Task<bool> CreateUser(PgUser user);
        Task<GetListUserResult> GetListUser(GetListUserParam param);
        Task<PgUser> UpdateUser(PgUser user);
        Task<PgCategoryUsageMoney?> GetCategoryInUserById(string categoryId, string userId);
        Task<List<PgCategoryUsageMoney>> GetListCategoryByUserId(string userId);
        Task<List<int>> GetTotalUserEachMonth(int year);
        Task<int> GetTotalUser();
        DashboardUserDataResult GetDashboard(string userId, string type, DateTime fromDate, DateTime toDate);
    }
}