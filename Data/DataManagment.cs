using System.Text.Json;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Data
{
    
    public class FileManager : IDataSaver, IDataLoader
    {
        public void SaveData(string filePath, User user, List<Food> foods)
        {
            var data = new { User = user, Foods = foods };
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        public (User, List<Food>) LoadData(string filePath)
        {
            if (!File.Exists(filePath))
                return (null, null);

            string jsonData = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<FoodDiaryData>(jsonData);
            return (data.User, data.Foods);
        }

        private class FoodDiaryData
        {
            public User User { get; set; }
            public List<Food> Foods { get; set; }
        }
    }
}