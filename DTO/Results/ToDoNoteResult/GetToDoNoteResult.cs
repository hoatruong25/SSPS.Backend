namespace DTO.Results.ToDoNoteResult
{
    public class GetToDoNoteResult : IResult
    {
        public GetToDoNoteDataResult? Data { get; set; }
    }

    public class GetToDoNoteDataResult
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Color { get; set; }
        public List<GetToDoNoteCardDataResult>? Cards { get; set; }
    }

    public class GetToDoNoteCardDataResult
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
