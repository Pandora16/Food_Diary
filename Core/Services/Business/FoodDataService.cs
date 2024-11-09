using Newtonsoft.Json;
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
            // Сохранение данных пользователя и продуктов в репозитории
            var allFoods = await _foodRepository.GetAllFoodsAsync();
            await _foodRepository.SaveDataAsync(user, allFoods);
        }

        public async Task<User> LoadUserAsync()
        {
            var jsonData = await File.ReadAllTextAsync("foodDiary.json");
            return JsonConvert.DeserializeObject<User>(jsonData);
        }
    }
}