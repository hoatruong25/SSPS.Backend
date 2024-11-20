using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
    public partial class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        public string Code { get; set; } = null!;
        // [BsonElement("Account")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? School { get; set; }
        public string? Location { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public string? Status { get; set; } = "ACTIVE";
        public List<CategoryUsageMoney>? CategoryUsageMoney { get; set; }
    }

    public class CategoryUsageMoney
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
        public ObjectId? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        public ObjectId? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }
}
