// Интерфейс для работы с файлами или базой данных

using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

public interface IDataManager
{
    Task SaveDataAsync(string filePath, User user, List<Food> foods);
    Task<(User, List<Food>)> LoadDataAsync(string filePath);
}