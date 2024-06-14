using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAngular.Api.Identity;
using TaskManagerAngular.Api.Models;
using TaskManagerAngular.Api.Services;

namespace TaskManagerAngular.Api.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUsersService m_UsersService;
    private readonly IAntiforgery m_Antiforgery;
    private readonly ApplicationSignInManager m_ApplicationSignInManager;

    public AccountController(
        IUsersService usersService,
        ApplicationSignInManager applicationSignManager,
        IAntiforgery antiforgery)
    {
        m_UsersService = usersService;
        m_ApplicationSignInManager = applicationSignManager;
        m_Antiforgery = antiforgery;
    }

    [HttpPost]
    [Route("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
    {
        var user = await m_UsersService.Authenticate(loginViewModel);
        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        //HttpContext.User = await m_ApplicationSignInManager.CreateUserPrincipalAsync(user);
        //var tokens = m_Antiforgery.GetAndStoreTokens(HttpContext);
        
        //Response.Headers.Append("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
        //Response.Headers.Append("XSRF-REQUEST-TOKEN", tokens.RequestToken);

        return Ok(user);
    }
}
