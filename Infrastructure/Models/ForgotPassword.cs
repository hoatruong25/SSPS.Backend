using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
    public partial class ForgotPassword
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        public string? Token { get; set; }
        public string Email { get; set; } = null!;
        public ObjectId UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
