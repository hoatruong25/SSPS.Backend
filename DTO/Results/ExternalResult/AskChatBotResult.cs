namespace DTO.Results.ExternalResult
{
    public class AskChatBotResult : IResult
    {
        public AskChatBotDataResult? data { get; set; }
    }
    public class AskChatBotDataResult
    {
        public string? type { get; set; }
        
        public string? response { get; set; }
    }
}
