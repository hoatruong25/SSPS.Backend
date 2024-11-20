namespace Infrastructure.PgModels
{
    public partial class PgOTP
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string OTP { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime UseDate { get; set; }
        public DateTime ExpireAt { get; set; }
        public string Type { get; set; } = null!;
        public bool IsUsed { get; set; }
    }
}