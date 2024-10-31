
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Interfaces
{
    public interface IFoodDiaryRepository
    {
        Task SaveDataAsync(string filePath, User user, List<Food> foods);
        Task<(User, List<Food>)> LoadDataAsync(string filePath);
    }
}