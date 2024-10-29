// Интерфейс для работы с файлами или базой данных

using Food_Diary.Models;
using Дневник_Питания.Meal;
using Дневник_Питания.Models;
using Дневник_Питания.UserManagment;

public interface IDataManager
{
    Task SaveDataAsync(string filePath, User user, List<FoodEntry> foods);
    Task<(User, List<FoodEntry>)> LoadDataAsync(string filePath);
}