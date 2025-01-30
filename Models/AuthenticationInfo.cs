using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.RegularExpressions;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
    public string? UserName { get; set; }

    [Required]
    [Phone] // Assuming you have a custom [Phone] attribute
    public string? PhoneNumber { get; set; }

    [Required]
    public string? PasswordHash { get; set; } 

    // Remove SetPassword and VerifyPassword methods
}

// Custom Phone attribute for validation
public class PhoneAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var phoneNumber = value as string;

        if (string.IsNullOrEmpty(phoneNumber))
        {
            return ValidationResult.Success; // Allow empty phone numbers
        }

        // Basic phone number validation (can be more sophisticated)
        var regex = new Regex(@"^\+?\d{10,15}$"); // Example: Allow 10-15 digits 
        if (!regex.IsMatch(phoneNumber))
        {
            return new ValidationResult("Invalid phone number format.", new[] { validationContext.MemberName });
        }

        return ValidationResult.Success;
    }
}