using Newtonsoft.Json;
using Дневник_Питания.DataManagement;
using Дневник_Питания.Meal;

namespace Food_Diary.Data
{
    public class JsonDataRepository : IDataRepository
    {
        private readonly string _filePath = "foodDiary.json";

        public async Task SaveDataAsync(FoodDiaryData diary)
        {
            var jsonData = JsonConvert.SerializeObject(diary);
            await File.WriteAllTextAsync(_filePath, jsonData);
        }

        public async Task<FoodDiaryData> LoadDataAsync()
        {
            if (!File.Exists(_filePath))
                return new FoodDiaryData();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<FoodDiaryData>(jsonData) ?? new FoodDiaryData();
        }
    }
}