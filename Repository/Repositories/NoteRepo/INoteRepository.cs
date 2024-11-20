using Infrastructure.Models;

namespace Repository.Repositories.NoteRepo
{
    public interface INoteRepository
    {
        Task<Note?> GetNoteById(string id);
        Task<List<Note>> GetListNoteInRange(string userId, DateTime fromDate, DateTime toDate);
        Task<Note> CreateNote(Note note);
        Task<Note> UpdateNote(Note note);
        Task<List<int>> GetTotalNoteEachMonth(int year);
    }
}