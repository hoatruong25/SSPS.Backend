using Infrastructure.Models;

namespace Repository.Repositories.ToDoNoteRepo
{
    public interface IToDoNoteRepository
    {
        Task<ToDoNote> CreateToDoNote(ToDoNote toDoNote);
        Task<ToDoNote> UpdateToDoNote(ToDoNote toDoNote);
        Task<ToDoNote> GetToDoNoteById(string id);
        Task<List<ToDoNote>> GetAllToDoNoteByUser(string userId);
    }
}