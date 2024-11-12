using System.Text.Json;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Models;
using Microsoft.Extensions.Logging;

namespace Дневник_Питания.Core.Repositories
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

        public async Task<User?> LoadUserAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    return JsonSerializer.Deserialize<User>(json);
                }
                else
                {
                    _logger.LogWarning("Файл не найден.");
                    return null;
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"Ошибка десериализации JSON: {jsonEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при загрузке данных пользователя: {ex.Message}");
                return null;
            }
        }


        public async Task<List<Food>> GetAllFoodsAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _logger.LogWarning("Файл данных не найден.");
                    return new List<Food>();
                }

                string jsonData = await File.ReadAllTextAsync(_filePath);
                var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);
                _logger.LogInformation("Список продуктов успешно загружен.");
                return data?.Foods ?? new List<Food>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при чтении списка продуктов: {ex.Message}");
                return new List<Food>();
            }
        }

        public async Task SaveFoodAsync(Food food)
        {
            try
            {
                var foods = await GetAllFoodsAsync();
                foods.Add(food);
                
                var user = await LoadUserAsync();
                await SaveDataAsync(user, foods);
                _logger.LogInformation("Продукт успешно добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении продукта: {ex.Message}");
                throw;
            }
        }

        private class FoodDiaryData
        {
            public User User { get; set; }
            public List<Food> Foods { get; set; }
        }
    }
}
