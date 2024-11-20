using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;

namespace Repository.PgReposiotries.PgToDoCardRepo
{
    public class PgToDoCardRepository : IPgToDoCardRepository
    {
        private readonly PgDbContext _dbContext;
        public PgToDoCardRepository(PgDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateListToDoCard(List<PgToDoCard> pgToDoCards)
        {
            _dbContext.ToDoCards.AddRange(pgToDoCards);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<PgToDoCard> CreateToDoCard(PgToDoCard pgToDoCard)
        {
            var result = _dbContext.ToDoCards.Add(pgToDoCard);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteToDoCard(string id)
        {
            var toDoCard = await _dbContext.ToDoCards.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (toDoCard == null)
                return;

            _dbContext.ToDoCards.Remove(toDoCard);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteToDoCardByToDoNoteId(string id)
        {
            var toDoCards = await _dbContext.ToDoCards.Where(x => x.ToDoNoteId == Guid.Parse(id)).ToListAsync();
            _dbContext.ToDoCards.RemoveRange(toDoCards);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PgToDoCard?> GetToDoCardById(string id)
        {
            var result = await _dbContext.ToDoCards.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return result;
        }

        public async Task<PgToDoCard> UpdateToDoCard(PgToDoCard pgToDoCard)
        {

            var result = _dbContext.ToDoCards.Update(pgToDoCard);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<PgToDoCard>> GetToDocardByToDoNoteId(Guid toDoNoteId)
        {
            return await _dbContext.ToDoCards.Where(x => x.ToDoNoteId == toDoNoteId).ToListAsync();
        }
    }
}
