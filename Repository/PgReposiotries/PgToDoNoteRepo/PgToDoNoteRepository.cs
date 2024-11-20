using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgToDoNoteRepo
{
    public class PgToDoNoteRepository : IPgToDoNoteRepository
    {
        private readonly PgDbContext _pgDbContext;
        public PgToDoNoteRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        public async Task<PgToDoNote> CreateToDoNote(PgToDoNote toDoNote)
        {
            var result = _pgDbContext.ToDoNotes.Add(toDoNote);
            await _pgDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<PgToDoNote>> GetAllToDoNoteByUser(string userId)
        {
            return await _pgDbContext.ToDoNotes.Where(x => x.UserId == Guid.Parse(userId) && x.IsDelete != true).ToListAsync();
        }

        public async Task<PgToDoNote?> GetToDoNoteById(string id)
        {
            return await _pgDbContext.ToDoNotes.Where(x => x.Id == Guid.Parse(id) && x.IsDelete == false).FirstOrDefaultAsync();
        }

        public async Task<PgToDoNote> UpdateToDoNote(PgToDoNote toDoNote)
        {
            var result = _pgDbContext.Update(toDoNote);
            await _pgDbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}