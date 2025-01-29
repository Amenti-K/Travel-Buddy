using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    // Inject UserService via Constructor
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET all users
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }

    // GET user by email
    [HttpGet("{email}")]
    public ActionResult<User> GetUserByEmail(string email)
    {
        var user = _userService.GetUserByEmail(email);
        if (user == null) return NotFound(new { message = "User not found" });
        return Ok(user);
    }

    // POST: Add a new user
    [HttpPost]
    public ActionResult<User> AddUser([FromBody] User newUser)
    {
        if (!_userService.AddUser(newUser))
        {
            return Conflict(new { message = "Email or phone number already exists" });
        }

        return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.Email }, newUser);
    }

    // PUT: Update user details
    [HttpPut("{email}")]
    public ActionResult UpdateUser(string email, [FromBody] User updatedUser)
    {
        if (!_userService.UpdateUser(email, updatedUser))
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new { message = "User updated successfully" });
    }

    // DELETE: Remove user by email
    [HttpDelete("{email}")]
    public ActionResult DeleteUser(string email)
    {
        if (!_userService.DeleteUser(email))
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new { message = "User deleted successfully" });
    }
}
