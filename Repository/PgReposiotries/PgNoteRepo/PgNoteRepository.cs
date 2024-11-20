using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgNoteRepo
{
    public class PgNoteRepository : IPgNoteRepository
    {
        private readonly PgDbContext _pgDbContext;
        public PgNoteRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        public async Task<PgNote> CreateNote(PgNote note)
        {
            var result = _pgDbContext.Notes.Add(note);
            await _pgDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<PgNote>> GetListNoteInRange(string userId, DateTime fromDate, DateTime toDate)
        {
            var resultData = await _pgDbContext.Notes.Where(x => x.UserId == Guid.Parse(userId) && x.IsDelete != true && ((x.FromDate.Date >= fromDate.Date && x.FromDate.Date <= toDate.Date) || (x.ToDate.Date >= fromDate.Date && x.ToDate.Date <= toDate.Date))).ToListAsync();

            return resultData;
        }

        public async Task<PgNote?> GetNoteById(string id)
        {
            var resultData = await _pgDbContext.Notes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.IsDelete == false);
            return resultData;
        }

        public async Task<List<int>> GetTotalNoteEachMonth(int year)
        {
            var notes = await _pgDbContext.Notes.Where(x => x.IsDelete == false && x.CreationTime.Value.Year == year).ToListAsync();
            var resultData = new List<int>
            {
                notes.Where(x => x.CreationTime.Value.Month == 1).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 2).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 3).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 4).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 5).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 6).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 7).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 8).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 9).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 10).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 11).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 12).Count(),
            };

            return resultData;
        }

        public async Task<PgNote> UpdateNote(PgNote note)
        {
            var resultData = _pgDbContext.Update(note);
            await _pgDbContext.SaveChangesAsync();

            return resultData.Entity;
        }
    }
}