using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key] // Marks this as the primary key
    public Guid UserId { get; set; } = Guid.NewGuid();

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }
}
