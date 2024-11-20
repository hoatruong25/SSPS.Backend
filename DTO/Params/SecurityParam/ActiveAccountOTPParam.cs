namespace DTO.Params.SecurityParam
{
    public class ActiveAccountOTPParam : IParam
    {
        public string Email { get; set; } = null!;
        public string OTP { get; set; } = null!;
    }
}