namespace DTO.Params.ToDoNoteParam
{
    public class CreateToDoCardParam : CreateToDoCardRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class CreateToDoCardRequest
    {
        public string ToDoNoteId { get; set; } = null!;
        public CreateToDoCardCardParam Card { get; set; } = new CreateToDoCardCardParam();
    }

    public class CreateToDoCardCardParam
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}