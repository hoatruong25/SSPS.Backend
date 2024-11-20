using Infrastructure.PgModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PgReposiotries.PgToDoCardRepo
{
    public interface IPgToDoCardRepository
    {
        Task<PgToDoCard?> GetToDoCardById(string id);
        Task CreateListToDoCard(List<PgToDoCard> pgToDoCards);
        Task<PgToDoCard> CreateToDoCard(PgToDoCard pgToDoCard);
        Task<PgToDoCard> UpdateToDoCard(PgToDoCard pgToDoCard);
        Task DeleteToDoCard(string id);
        Task DeleteToDoCardByToDoNoteId(string id);
        Task<List<PgToDoCard>> GetToDocardByToDoNoteId(Guid toDoNoteId);
    }
}
