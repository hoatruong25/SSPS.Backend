namespace DTO.Params.NoteParam
{
    public class UpdateNoteParam : UpdateNoteRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class UpdateNoteRequest
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}