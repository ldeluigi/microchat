using EasyDesk.CleanArchitecture.Web.Controllers;

namespace AuthService.Web.Controllers;

public class AuthServiceApiVersionController : VersionsController
{
    public AuthServiceApiVersionController() : base(typeof(Startup))
    {
    }
}
