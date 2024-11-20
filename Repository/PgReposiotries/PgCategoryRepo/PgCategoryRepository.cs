using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PgReposiotries.PgCategoryRepo
{
    public class PgCategoryRepository : IPgCategoryRepository
    {
        private readonly PgDbContext _dbContext;
        public PgCategoryRepository(PgDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateListCategory(List<PgCategoryUsageMoney> categories)
        {
            _dbContext.Categories.AddRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryByUserId(string id)
        {
            var categories = await _dbContext.Categories.Where(x => x.UserId == Guid.Parse(id)).ToListAsync();
            _dbContext.Categories.RemoveRange(categories);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryById(string Id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == Guid.Parse(Id));
            if (category == null)
            {
                return;
            }
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
