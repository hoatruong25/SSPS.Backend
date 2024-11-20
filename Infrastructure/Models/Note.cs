using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
    public partial class Note
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Color { get; set; }
        public ObjectId UserId { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? DeletorId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool? IsDelete { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
    }
}
