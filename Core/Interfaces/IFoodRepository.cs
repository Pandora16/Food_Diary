using System.Collections.Generic;
using System.Threading.Tasks;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IFoodRepository
    {
        Task SaveFoodAsync(Food food);
        Task<List<Food>> GetAllFoodsAsync(); // Убрали параметр filePath
        Task SaveDataAsync(User user, List<Food> foods); // Убрали параметр filePath
        Task<User> LoadUserAsync(); // Убрали параметр filePath
    }
}