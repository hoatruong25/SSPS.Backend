using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgMoneyPlan
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Type { get; set; } = null!; // Day, month, year
        public string? Status { get; set; }
        public double ExpectAmount { get; set; }
        public double? ActualAmount { get; set; }
        public string? CurrencyUnit { get; set; }
        public double? TotalChildrenMoney { get; set; } // Modify every update and create child
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        // Cap nhat money plan
        public DateTime? Date { get; set; }

        public Guid? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }
}
