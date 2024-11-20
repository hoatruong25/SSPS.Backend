namespace DTO.Results.ToDoNoteResult
{
    public class CreateToDoCardResult : IResult
    {
        public CreateToDoCardDataResult? Data { get; set; }
    }

    public class CreateToDoCardDataResult
    {
        public string? Id { get; set; }
    }
}
