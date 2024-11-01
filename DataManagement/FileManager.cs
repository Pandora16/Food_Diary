using System.Text.Json;
using Food_Diary.Models;
using Дневник_Питания.Models;
using Дневник_Питания.UserManagment;

public class FileManager : IDataManager
{
    public async Task SaveDataAsync(string filePath, User user, List<FoodEntry> foods)
    {
        var data = new { User = user, Foods = foods };
        string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, jsonData);
    }

    public async Task<(User, List<FoodEntry>)> LoadDataAsync(string filePath)
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
        public List<FoodEntry> Foods { get; set; }
    }
}