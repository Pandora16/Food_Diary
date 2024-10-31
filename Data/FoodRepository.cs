using System.Text.Json;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Interfaces;

namespace Дневник_Питания.Data;

public class FoodDiaryRepository : IFoodDiaryRepository
{
    public async Task SaveDataAsync(string filePath, User user, List<Food> foods)
    {
        var data = new { User = user, Foods = foods };
        string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, jsonData);
    }

    public async Task<(User, List<Food>)> LoadDataAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return (null, null);

        string jsonData = await File.ReadAllTextAsync(filePath);
        var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);
        return (data.User, data.Foods);
    }

    private class FoodDiaryData
    {
        public User User { get; set; }
        public List<Food> Foods { get; set; }
    }
}