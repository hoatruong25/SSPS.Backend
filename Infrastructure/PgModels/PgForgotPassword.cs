using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgForgotPassword
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Token { get; set; }
        public string Email { get; set; } = null!;
        public bool? IsSuccess { get; set; }


        public Guid? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }
}
