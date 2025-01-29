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
        return Ok(await _userService.GetAllUsers());
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmail(email);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> AddUser([FromBody] User user)
    {
        var added = await _userService.AddUser(user);
        if (!added) return Conflict("User with this email already exists.");
        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
    }

    [HttpPut("{email}")]
    public async Task<ActionResult> UpdateUser(string email, [FromBody] User user)
    {
        if (!await _userService.UpdateUser(email, user)) return NotFound();
        return Ok();
    }

    [HttpDelete("{email}")]
    public async Task<ActionResult> DeleteUser(string email)
    {
        if (!await _userService.DeleteUser(email)) return NotFound();
        return Ok();
    }
}
