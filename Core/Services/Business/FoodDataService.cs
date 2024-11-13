using System.Threading.Tasks;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business
{
    public class FoodDataService : IFoodDataService
    {
        private readonly IFoodRepository _foodRepository;

        public FoodDataService(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task SaveAllDataAsync(User user)
        {
            var allFoods = await _foodRepository.GetAllFoodsAsync();
            await _foodRepository.SaveDataAsync(user, allFoods);
        }

        public async Task<User> LoadUserAsync()
        {
            var (user, _) = await _foodRepository.LoadDataAsync(); 
            return user ?? new User();
        }
    }
}