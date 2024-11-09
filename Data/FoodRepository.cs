using System.Text.Json;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly string _filePath;

        public FoodRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task SaveDataAsync(User user, List<Food> foods)
        {
            var data = new FoodDiaryData { User = user, Foods = foods };
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonData);
        }

        public async Task<User> LoadUserAsync()
        {
            try
            {
                if (!File.Exists(_filePath)) return null;
                string jsonData = await File.ReadAllTextAsync(_filePath);
                var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);
                return data?.User;
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                return null;
            }
        }
        
        public async Task<List<Food>> GetAllFoodsAsync()
        {
            if (!File.Exists(_filePath)) return new List<Food>();

            string jsonData = await File.ReadAllTextAsync(_filePath);
            var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);
            return data?.Foods ?? new List<Food>();
        }

        public async Task SaveFoodAsync(Food food)
        {
            var foods = await GetAllFoodsAsync();
            foods.Add(food);

            var user = await LoadUserAsync();
            await SaveDataAsync(user, foods);
        }

        private class FoodDiaryData
        {
            public User User { get; set; }
            public List<Food> Foods { get; set; }
        }
    }
}