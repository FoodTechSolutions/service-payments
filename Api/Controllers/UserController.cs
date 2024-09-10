using Domain.Boundaries.Invoices.DeleteUser;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("/delete-account")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult> ProcessUser([FromBody] DeleteUserRequest request)
    {
        try
        {
            await _userService.Delete(request.UserId);
        }
        catch (Exception e)
        {
            return BadRequest(
                new ProblemDetails
                {
                    Title = e.Message,
                    Status = StatusCodes.Status400BadRequest
                }
            );
        }

        return Ok();
    }
}
