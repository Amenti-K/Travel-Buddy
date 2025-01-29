using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    // Dummy list of users (acting as a database)
    private static List<User> users = new List<User>
    {
        new User { UserId = Guid.NewGuid(), Email = "john@example.com", UserName = "JohnDoe", PhoneNumber = "1234567890", Password = "pass123" },
        new User { UserId = Guid.NewGuid(), Email = "jane@example.com", UserName = "JaneDoe", PhoneNumber = "0987654321", Password = "pass456" },
        new User { UserId = Guid.NewGuid(), Email = "alex@example.com", UserName = "AlexSmith", PhoneNumber = "1122334455", Password = "pass789" }
    };

    // GET all users
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAllUsers()
    {
        return Ok(users);
    }

    // GET user by email
    [HttpGet("{email}")]
    public ActionResult<User> GetUserByEmail(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return NotFound(new { message = "User not found" });
        return Ok(user);
    }

    // POST: Add a new user
    [HttpPost]
    public ActionResult<User> AddUser([FromBody] User newUser)
    {
        if (users.Any(u => u.Email == newUser.Email || u.PhoneNumber == newUser.PhoneNumber))
        {
            return Conflict(new { message = "Email or phone number already exists" });
        }

        newUser.UserId = Guid.NewGuid();
        users.Add(newUser);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.Email }, newUser);
    }

    // PUT: Update user details
    [HttpPut("{email}")]
    public ActionResult UpdateUser(string email, [FromBody] User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return NotFound(new { message = "User not found" });

        // Update fields
        user.UserName = updatedUser.UserName;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Password = updatedUser.Password;

        return Ok(new { message = "User updated successfully", user });
    }

    // DELETE: Remove user by email
    [HttpDelete("{email}")]
    public ActionResult DeleteUser(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null) return NotFound(new { message = "User not found" });

        users.Remove(user);
        return Ok(new { message = "User deleted successfully" });
    }
}
