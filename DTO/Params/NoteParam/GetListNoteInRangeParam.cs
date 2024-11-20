namespace DTO.Params.NoteParam
{
    public class GetListNoteInRangeParam : GetListNoteInRangeRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class GetListNoteInRangeRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}