using EasyDesk.Tools.Options;

namespace UserService.Web.Controllers.V_1_0.DTO;

public record ModifyUserBodyDto(string Name, string Surname, string Email);
