using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgNote
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Color { get; set; }


        public Guid? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }
}
