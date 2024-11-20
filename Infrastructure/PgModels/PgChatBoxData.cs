using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PgModels
{
    public partial class PgChatBoxData
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MoneyPlanId { get; set; }
        public string Username { get; set; } = null!;
        public string Category { get; set; } = null!;
        public double Amount { get; set; } = 0; // Pick ActualAmount if ActualAmount is not null (default is ExpectAmount)
        public DateTime Date { get; set; }
    }
}