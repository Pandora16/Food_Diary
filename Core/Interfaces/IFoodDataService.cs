using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces;

public interface IFoodDataService
{
    Task SaveAllDataAsync(User user);
}