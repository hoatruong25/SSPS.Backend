namespace DTO.Params.ToDoNoteParam
{
    public class UpdateToDoCardParam : UpdateToDoCardRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class UpdateToDoCardRequest
    {
        public string ToDoNoteId { get; set; } = null!;
        public string CardId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}