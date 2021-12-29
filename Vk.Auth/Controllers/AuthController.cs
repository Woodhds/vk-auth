using Microsoft.AspNetCore.Mvc;

namespace Vk.Auth.Controllers;

[ApiController]
[Route("Auth")]
public class AuthController : ControllerBase
{
    [HttpGet("Login")]
    public IActionResult Auth()
    {
        return Challenge("vkontakte");
    }
}