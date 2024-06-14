using TaskManagerAngular.Api.Identity;
using TaskManagerAngular.Api.Models;

namespace TaskManagerAngular.Api.Services;

public interface IUsersService
{
    Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
}
