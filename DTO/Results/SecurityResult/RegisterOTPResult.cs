namespace DTO.Results.SecurityResult
{
    public class RegisterOTPResult : IResult
    {
        public RegisterOTPDataResult? Data { get; set; }
    }
    public class RegisterOTPDataResult
    {
        public string Id { get; set; } = null!;
    }
}