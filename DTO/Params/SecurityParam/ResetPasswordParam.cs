namespace DTO.Params.SecurityParam
{
    public class ResetPasswordParam : IParam
    {
        public string Token { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}