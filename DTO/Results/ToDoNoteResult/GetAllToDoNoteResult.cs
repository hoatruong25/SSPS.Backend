namespace DTO.Results.ToDoNoteResult
{
    public class GetAllToDoNoteResult : IResult
    {
        public List<GetAllToDoNoteDataResult>? Data { get; set; }
    }

    public class GetAllToDoNoteDataResult
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Color { get; set; }
        public List<GetAllToDoNoteCardDataResult>? Cards { get; set; }
    }

    public class GetAllToDoNoteCardDataResult
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
