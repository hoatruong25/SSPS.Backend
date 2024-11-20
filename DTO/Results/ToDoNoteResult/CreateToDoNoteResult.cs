namespace DTO.Results.ToDoNoteResult
{
    public class CreateToDoNoteResult : IResult
    {
        public CreateToDoNoteDataResult? Data { get; set; }
    }

    public class CreateToDoNoteDataResult
    {
        public string? Id { get; set; }
    }
}
