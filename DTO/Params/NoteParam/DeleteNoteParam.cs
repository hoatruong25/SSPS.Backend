namespace DTO.Params.NoteParam
{
    public class DeleteNoteParam : DeleteNoteRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class DeleteNoteRequest
    {
        public string Id { get; set; } = null!;
    }
}