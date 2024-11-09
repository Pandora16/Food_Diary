using Microsoft.Extensions.Logging;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business;

public class FoodService : IFoodService
{
    private readonly IFoodManagementService _foodManagementService;
    private readonly IFoodDataService _foodDataService;
    private readonly ILogger<FoodService> _logger;

    public FoodService(IFoodManagementService foodManagementService, IFoodDataService foodDataService, ILogger<FoodService> logger)
    {
        _foodManagementService = foodManagementService;
        _foodDataService = foodDataService;
        _logger = logger;
    }

    public async Task AddFoodAsync()
    {
        try
        {
            _logger.LogInformation("Добавление нового продукта.");
            await _foodManagementService.AddFoodAsync();
            _logger.LogInformation("Продукт успешно добавлен.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при добавлении продукта: {ex.Message}");
            throw;
        }
    }

    public async Task SaveAllDataAsync(User user)
    {
        try
        {
            _logger.LogInformation("Сохранение данных пользователя.");
            await _foodDataService.SaveAllDataAsync(user);
            _logger.LogInformation("Данные успешно сохранены.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при сохранении данных: {ex.Message}");
            throw;
        }
    }
}