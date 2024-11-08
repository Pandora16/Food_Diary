using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IFoodRepository
    {
        Task SaveFoodAsync(Food food);
        Task<List<Food>> GetAllFoodsAsync(); 
        Task SaveDataAsync(User user, List<Food> foods); 
        Task<User> LoadUserAsync();
    }
}