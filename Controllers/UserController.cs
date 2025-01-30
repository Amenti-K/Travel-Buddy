using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null) return NotFound();
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        try
        {
            // Validate the user input
            var (isValid, errors) = ValidationService.ValidateUser(user);
            if (!isValid) return BadRequest(errors);

            var added = await _userService.AddUser(user);
            if (!added) return Conflict("User with this email already exists.");
            return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{email}")]
    public async Task<ActionResult> UpdateUser(string email, [FromBody] User user)
    {
        try
        {
            // Validate the user input
            var (isValid, errors) = ValidationService.ValidateUser(user);
            if (!isValid) return BadRequest(errors);

            var updated = await _userService.UpdateUser(email, user);
            if (!updated) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{email}")]
    public async Task<ActionResult> DeleteUser(string email)
    {
        try
        {
            var deleted = await _userService.DeleteUser(email);
            if (!deleted) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}