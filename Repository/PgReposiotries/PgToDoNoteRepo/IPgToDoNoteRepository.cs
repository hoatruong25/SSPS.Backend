using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgToDoNoteRepo
{
    public interface IPgToDoNoteRepository
    {
        Task<PgToDoNote> CreateToDoNote(PgToDoNote toDoNote);
        Task<PgToDoNote> UpdateToDoNote(PgToDoNote toDoNote);
        Task<PgToDoNote?> GetToDoNoteById(string id);
        Task<List<PgToDoNote>> GetAllToDoNoteByUser(string userId);
    }
}