using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgToDoCard
    {
        public Guid Id { get; set; }
        public Guid? ToDoNoteId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
