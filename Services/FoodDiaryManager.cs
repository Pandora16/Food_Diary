using Food_Diary.Models;
using Дневник_Питания.DataManagement;
using Дневник_Питания.Interface;
using Дневник_Питания.Interfaces;
using Дневник_Питания.Meal;
using Дневник_Питания.Models;

namespace Дневник_Питания.Services;

// FoodDiaryManager.cs
public class FoodDiaryManager : IFoodDiaryManager
{
    private readonly IDataRepository _dataRepository;
    private readonly IUserInterface _userInterface;
    private readonly FoodDiaryData _foodDiary;

    public FoodDiaryManager(IDataRepository dataRepository, IUserInterface userInterface)
    {
        _dataRepository = dataRepository;
        _userInterface = userInterface;
        _foodDiary = new FoodDiaryData();
    }

    public async Task AddFoodAsync(string name, int calories, double protein, double fats, double carbs, string mealType)
    {
        var entry = new FoodEntry
        {
            Name = name,
            Calories = calories,
            Proteins = protein,
            Fats = fats,
            Carbohydrates = carbs,
            MealType = mealType,
            Date = DateTime.Now
        };
        _foodDiary.AddFood();
        await _dataRepository.SaveDataAsync(_foodDiary);
        await _userInterface.WriteMessageAsync($"Food {name} added successfully!");
    }

    public async Task ShowStatisticsAsync()
    {
        //var totalCalories = _foodDiary.Entries.Sum(e => e.Calories);
        //await _userInterface.WriteMessageAsync($"Total calories consumed: {totalCalories}");
    }

    public async Task UpdateUserProfileAsync(int height, int weight)
    {
        _foodDiary.User = new User { Height = height, Weight = weight };
        await _dataRepository.SaveDataAsync(_foodDiary);
        await _userInterface.WriteMessageAsync("User profile updated.");
    }

    public Task AddFoodAsync(string name, int calories)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFoodAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task ShowSummaryAsync()
    {
        throw new NotImplementedException();
    }
}
