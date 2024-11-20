using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgForgotPasswordRepo
{
    public interface IPgForgotPasswordRepository
    {
        Task<PgForgotPassword> CreateForgotPasswordAsync(PgForgotPassword forgotPassword);
        Task<PgForgotPassword?> GetForgotPasswordByTokenAsync(string token);
        Task<bool> UpdateForgotPassword(PgForgotPassword forgotPassword);
    }
}