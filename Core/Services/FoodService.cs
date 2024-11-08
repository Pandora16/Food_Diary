using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;

public class FoodService : IFoodService
{
    private readonly IFoodManagementService _foodManagementService;
    private readonly IFoodDataService _foodDataService;

    public FoodService(IFoodManagementService foodManagementService, IFoodDataService foodDataService)
    {
        _foodManagementService = foodManagementService;
        _foodDataService = foodDataService;
    }

    public async Task AddFoodAsync()
    {
        await _foodManagementService.AddFoodAsync();
    }

    public async Task SaveAllDataAsync(User user)
    {
        await _foodDataService.SaveAllDataAsync(user);
    }
}