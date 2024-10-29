using Дневник_Питания.DataManagement;
using Дневник_Питания.Interface;

namespace Дневник_Питания.Services;

// FoodDiaryManager.cs
public class FoodDiaryManager
{
    private readonly IDataRepository _dataRepository;
    private readonly IUserInterface _userInterface;

    public FoodDiaryManager(IDataRepository dataRepository, IUserInterface userInterface)
    {
        _dataRepository = dataRepository;
        _userInterface = userInterface;
    }

    public async Task AddFoodAsync(string foodName, int calories)
    {
        // Вся логика добавления еды без консольных вызовов
    }
}
