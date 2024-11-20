using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgNoteRepo
{
    public interface IPgNoteRepository
    {
        Task<PgNote?> GetNoteById(string id);
        Task<List<PgNote>> GetListNoteInRange(string userId, DateTime fromDate, DateTime toDate);
        Task<PgNote> CreateNote(PgNote note);
        Task<PgNote> UpdateNote(PgNote note);
        Task<List<int>> GetTotalNoteEachMonth(int year);
    }
}