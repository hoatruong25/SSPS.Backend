using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
    public partial class MoneyPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId UserId { get; set; }
        public string Type { get; set; } = null!; // Day, month, year
        public string Status { get; set; } = null!;
        public double ExpectAmount { get; set; }
        public double? ActualAmount { get; set; }
        public string? CurrencyUnit { get; set; }
        public double? TotalChildrenMoney { get; set; } // Modify every update and create child
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public List<UsageMoney>? UsageMoneys { get; set; }
        public List<MoneyPlanDetail>? Details { get; set; } // Apply for day type



        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }

    public class UsageMoney
    {
        public ObjectId _id { get; set; }
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int? Priority { get; set; }
        public CategoryUsageMoney? Category { get; set; }
    }

    public class MoneyPlanDetail
    {
        public ObjectId _id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Title { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
    }
}
