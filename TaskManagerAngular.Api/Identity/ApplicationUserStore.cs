using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskManagerAngular.Api.Identity;

public class ApplicationUserStore : UserStore<ApplicationUser>
{
    public ApplicationUserStore(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
}