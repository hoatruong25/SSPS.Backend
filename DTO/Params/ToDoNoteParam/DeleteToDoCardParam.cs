namespace DTO.Params.ToDoNoteParam
{
    public class DeleteToDoCardParam : DeleteToDoCardRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class DeleteToDoCardRequest
    {
        public string ToDoNoteId { get; set; } = null!;
        public string CardId { get; set; } = null!;
    }
}