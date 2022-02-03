using EasyDesk.CleanArchitecture.Web.Controllers;

namespace ChatService.Web.Controllers;

public class ChatServiceApiVersionController : VersionsController
{
    public ChatServiceApiVersionController() : base(typeof(Startup))
    {
    }
}
