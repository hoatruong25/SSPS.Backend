namespace Infrastructure.PgModels
{
    public partial class PgUser
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? School { get; set; }
        public string? Location { get; set; }
        public string? DeviceToken { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public string? Status { get; set; } = "ACTIVE";
    }
}
