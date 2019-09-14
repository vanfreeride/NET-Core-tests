using System.ComponentModel.DataAnnotations;
using jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authService;
    public AuthController (AuthenticationService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult RequestToken ([FromBody] TokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest ("Invalid Request");
        }

        string token;

        if (_authService.IsAuthenticated(request, out token))
        {
            return Ok(token); 
        }

        return BadRequest("Invalid Request");

    }
}

public class TokenRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}