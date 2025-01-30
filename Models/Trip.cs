using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Trip
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TripId { get; set; }

    [BsonRequired]
    [Required(ErrorMessage = "CreatorId is required.")]
    public string CreaterId { get; set; } = string.Empty;

    [BsonRequired]
    [Required(ErrorMessage = "TripName is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "TripName must be between 3 and 100 characters.")]
    public string TripName { get; set; } = string.Empty;

    public string? TripDescription { get; set; }

    [BsonRequired]
    [Required(ErrorMessage = "DepartureDate is required.")]
    [FutureDate(ErrorMessage = "DepartureDate must be in the future.")]
    public DateTime DepartureDate { get; set; }

    [ReturnDateValidation("DepartureDate", ErrorMessage = "ReturnDate must be after DepartureDate.")]
    public DateTime? ReturnDate { get; set; }

    [BsonRequired]
    [Required(ErrorMessage = "StartingLocation is required.")]
    public string StartingLocation { get; set; } = string.Empty;

    [BsonRequired]
    [Required(ErrorMessage = "Destination is required.")]
    public string Destination { get; set; } = string.Empty;
}
