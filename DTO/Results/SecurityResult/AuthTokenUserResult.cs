namespace DTO.Results.SecurityResult
{
    public class AuthTokenUserResult : IResult
    {
        public bool Result { get; set; }
        public string? AccessToken { get; set; }
        public long? Expires { get; set; }
        public string? Id { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
        public string? Role { get; set; }

    }
}
