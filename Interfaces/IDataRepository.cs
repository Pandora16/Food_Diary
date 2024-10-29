using Дневник_Питания.Meal;

namespace Дневник_Питания.DataManagement;

public interface IDataRepository
{
    Task SaveDataAsync(FoodDiaryData diary);
    Task<FoodDiaryData> LoadDataAsync();
}