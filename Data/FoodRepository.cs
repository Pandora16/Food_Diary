using System.Text.Json;
using Microsoft.Extensions.Logging;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Data
{
    public class FoodRepository : IFoodRepository
    {
        private readonly string _filePath;
        private readonly ILogger<FoodRepository> _logger;

        public FoodRepository(string filePath, ILogger<FoodRepository> logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        public async Task SaveDataAsync(User user, List<Food> foods)
        {
            try
            {
                var data = new FoodDiaryData { User = user, Foods = foods };
                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, jsonData);
                _logger.LogInformation("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при сохранении данных: {ex.Message}");
                throw;
            }
        }

        public async Task<(User?, List<Food>)> LoadDataAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonData = await File.ReadAllTextAsync(_filePath);
                    var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);

                    if (data == null)
                    {
                        _logger.LogWarning("Файл пуст или структура данных некорректна.");
                        return (null, new List<Food>());
                    }

                    _logger.LogInformation("Данные пользователя и продукты успешно загружены.");
                    return (data.User, data.Foods);
                }
                else
                {
                    _logger.LogWarning("Файл данных не найден.");
                    return (null, new List<Food>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при чтении файла: {ex.Message}");
                return (null, new List<Food>());
            }
        }

        public async Task SaveFoodAsync(Food food)
        {
            try
            {
                var (user, foods) = await LoadDataAsync();
                foods.Add(food);
                await SaveDataAsync(user, foods);
                _logger.LogInformation("Продукт успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении продукта: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            var (_, foods) = await LoadDataAsync();
            return foods;
        }

        public class FoodDiaryData
        {
            public User User { get; set; } = new User();
            public List<Food> Foods { get; set; } = new List<Food>();
        }
    }
}