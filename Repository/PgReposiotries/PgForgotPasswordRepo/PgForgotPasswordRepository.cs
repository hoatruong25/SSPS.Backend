using Infrastructure.Models;
using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgForgotPasswordRepo
{
    public class PgForgotPasswordRepository : IPgForgotPasswordRepository
    {
        private readonly PgDbContext _pgDbContext;
        public PgForgotPasswordRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        public async Task<PgForgotPassword> CreateForgotPasswordAsync(PgForgotPassword forgotPassword)
        {
            var result = _pgDbContext.ForgotPasswords.Add(forgotPassword);
            await _pgDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<PgForgotPassword?> GetForgotPasswordByTokenAsync(string token)
        {
            return await _pgDbContext.ForgotPasswords.Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateForgotPassword(PgForgotPassword forgotPassword)
        {
            _pgDbContext.Update(forgotPassword);
            await _pgDbContext.SaveChangesAsync();

            return true;
        }
    }
}
