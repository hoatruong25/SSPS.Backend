namespace DTO.Results.NoteResult
{
    public class GetListNoteInRangeResult : IResult
    {
        public List<GetListNoteInRangeDataResult>? Data { get; set; }
    }

    public class GetListNoteInRangeDataResult
    {
        public string? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Color { get; set; }
    }
}
