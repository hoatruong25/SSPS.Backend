using Infrastructure.PgModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PgReposiotries.PgCategoryRepo
{
    public interface IPgCategoryRepository
    {
        Task CreateListCategory(List<PgCategoryUsageMoney> categories);
        Task DeleteCategoryByUserId(string id);
        Task DeleteCategoryById(string Id);
    }
}
