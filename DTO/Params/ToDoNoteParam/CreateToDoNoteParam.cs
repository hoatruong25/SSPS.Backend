namespace DTO.Params.ToDoNoteParam
{
    public class CreateToDoNoteParam : CreateToDoNoteRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class CreateToDoNoteRequest
    {
        public string Title { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Color { get; set; }
        public List<CreateToDoNoteCardParam>? Cards { get; set; }
    }

    public class CreateToDoNoteCardParam
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}