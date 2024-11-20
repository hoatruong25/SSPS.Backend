namespace DTO.Results.NoteResult
{
    public class CreateNoteResult : IResult
    {
        public CreateNoteDataResult? Data { get; set; }
    }

    public class CreateNoteDataResult
    {
        public string? Id { get; set; }
    }
}
