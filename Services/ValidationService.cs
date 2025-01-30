using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidationService
{
    public static (bool IsValid, Dictionary<string, string> Errors) ValidateUser(User user)
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(user.Email) || !new EmailAddressAttribute().IsValid(user.Email))
        {
            errors.Add("Email", "Invalid email format. Example: example@domain.com");
        }

        if (string.IsNullOrWhiteSpace(user.UserName) || !Regex.IsMatch(user.UserName, @"[a-zA-Z]"))
        {
            errors.Add("UserName", "Username must contain at least one letter.");
        }

        if (string.IsNullOrWhiteSpace(user.PhoneNumber) || !new PhoneAttribute().IsValid(user.PhoneNumber))
        {
            errors.Add("PhoneNumber", "Invalid phone number format.");
        }

        if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8)
        {
            errors.Add("Password", "Password must be at least 8 characters long.");
        }

        return (errors.Count == 0, errors);
    }

    public static (bool IsValid, Dictionary<string, string> Errors) ValidateTrip(Trip trip)
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(trip.TripName) || trip.TripName.Length < 5 || !Regex.IsMatch(trip.TripName, @"[a-zA-Z]"))
        {
            errors.Add("TripName", "Trip name must be at least 5 characters long and contain letters.");
        }

        if (!string.IsNullOrWhiteSpace(trip.TripDescription) && trip.TripDescription.Length > 250)
        {
            errors.Add("TripDescription", "Trip description cannot exceed 250 characters.");
        }

        if (trip.DepartureDate == default)
        {
            errors.Add("DepartureDate", "Invalid departure date format.");
        }

        if (trip.ReturnDate.HasValue && trip.ReturnDate < trip.DepartureDate)
        {
            errors.Add("ReturnDate", "Return date cannot be before the departure date.");
        }

        if (string.IsNullOrWhiteSpace(trip.StartingLocation))
        {
            errors.Add("StartingLocation", "Starting location must be a valid address.");
        }

        if (string.IsNullOrWhiteSpace(trip.Destination))
        {
            errors.Add("Destination", "Destination must be a valid address.");
        }

        return (errors.Count == 0, errors);
    }
}
