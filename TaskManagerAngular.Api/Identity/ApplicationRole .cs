using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerAngular.Api.Identity;

//[NotMapped]
public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }
}
