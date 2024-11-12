using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces.Services;

public interface IUserDataInitializer
{
    Task<User> InitializeUserDataAsync();
}