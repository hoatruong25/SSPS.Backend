using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgUsageMoney
    {
        public Guid Id { get; set; }
        public Guid? MoneyPlanId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int? Priority { get; set; }
    }
}
