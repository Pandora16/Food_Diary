using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces.Services;

public interface IUserCreator
{
    Task<User> CreateNewUserAsync();
}