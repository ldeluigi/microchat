using EasyDesk.CleanArchitecture.Web.Controllers;

namespace UserService.Web.Controllers;

public class UserServiceApiVersionController : VersionsController
{
    public UserServiceApiVersionController() : base(typeof(Startup))
    {
    }
}
