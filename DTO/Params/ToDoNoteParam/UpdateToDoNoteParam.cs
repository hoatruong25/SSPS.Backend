namespace DTO.Params.ToDoNoteParam
{
    public class UpdateToDoNoteParam : UpdateToDoNoteRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class UpdateToDoNoteRequest
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Color { get; set; }
        public List<UpdateToDoNoteCardParam>? Cards { get; set; }
    }

    public class UpdateToDoNoteCardParam
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}