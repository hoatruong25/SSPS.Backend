namespace DTO.Params.ToDoNoteParam
{
    public class SwapToDoCardParam : IParam
    {
        public string CardId { get; set; } = null!;

        public string FromToDoNoteId { get; set; } = null!;
        public string ToToDoNoteId { get; set; } = null!;
    }
}