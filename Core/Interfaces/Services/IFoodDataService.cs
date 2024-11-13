using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces.Services;

public interface IFoodDataService
{
    Task SaveAllDataAsync(User user); // Сохранение данных
    Task<User> LoadUserAsync(); // Загрузка пользователя из данных
}