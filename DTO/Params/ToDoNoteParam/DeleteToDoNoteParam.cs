namespace DTO.Params.ToDoNoteParam
{
    public class DeleteToDoNoteParam : DeleteToDoNoteRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class DeleteToDoNoteRequest
    {
        public string Id { get; set; } = null!;
    }
}