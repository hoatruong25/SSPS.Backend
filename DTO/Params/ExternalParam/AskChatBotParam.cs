namespace DTO.Params.ExternalParam
{
    public class AskChatBotParam : IParam
    {
        public string Message { get; set; } = null!;
        public string? UserId { get; set; }
    }
}
