using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace TaskManagerAngular.Api.Identity;

public class ApplicationSignInManager : SignInManager<ApplicationUser>
{
    public ApplicationSignInManager(
        ApplicationUserManager applicationUserManager,
        IHttpContextAccessor httpContextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IOptions<IdentityOptions> options,
        ILogger<ApplicationSignInManager> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation) 
        : base(applicationUserManager, httpContextAccessor, userClaimsPrincipalFactory, options, logger, schemes, confirmation)
    {

    }
}