using EasyDesk.CleanArchitecture.Domain.Model;

namespace UserService.Domain.Aggregates.UserService;

public interface INameService
{
    bool IsCorrectName(string nameString, Name name);
}
