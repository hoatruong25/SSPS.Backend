using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgChatBoxDataRepo
{
    public class PgChatBoxDataRepository : IPgChatBoxDataRepository
    {
        private readonly PgDbContext _context;
        public PgChatBoxDataRepository(PgDbContext context)
        {
            _context = context;
        }
        public async Task CreateChatBoxData(PgChatBoxData entity)
        {
            _context.ChatBoxData.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatBoxData(PgChatBoxData entity)
        {
            _context.ChatBoxData.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PgChatBoxData?> GetChatBoxDataByUsageId(Guid moneyPlanId)
        {
            return await _context.ChatBoxData.FirstOrDefaultAsync(x => x.MoneyPlanId == moneyPlanId);
        }

        public async Task UpdateChatBoxData(PgChatBoxData entity)
        {
            _context.ChatBoxData.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}