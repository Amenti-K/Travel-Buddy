using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

 public class Trip
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? TripId { get; set; }

        [BsonRequired]
        public string CreaterId { get; set; } = string.Empty;

        [BsonRequired]
        public string TripName { get; set; } = string.Empty;

        public string? TripDescription { get; set; }

        [BsonRequired]
        public DateTime DepartureDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [BsonRequired]
        public string StartingLocation { get; set; } = string.Empty;

        [BsonRequired]
        public string Destination { get; set; } = string.Empty;
    }