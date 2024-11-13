using Microsoft.Extensions.Logging;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using Дневник_Питания.Core.Services.Business;
using Дневник_Питания.Core.Services.Utility;
using Дневник_Питания.Data;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = loggerFactory.CreateLogger("General");

            logger.LogInformation("Программа запущена.");

            // Ручное создание экземпляров классов
            var userInterface = new ConsoleUserInterface();
            var inputManager = new UserInputManager(userInterface);
            var calorieCalculator = new CalorieCalculator();
            var foodRepository = new FoodRepository("foodDiary.json", loggerFactory.CreateLogger<FoodRepository>());
            var userCreator = new UserCreator(inputManager, calorieCalculator, loggerFactory.CreateLogger<UserCreator>());
            var userDataInitializer = new UserDataInitializer(foodRepository, userCreator, loggerFactory.CreateLogger<UserDataInitializer>());
            var foodManagementService = new FoodManagementService(foodRepository, inputManager, userInterface);
            var foodService = new FoodService(foodManagementService, new FoodDataService(foodRepository), loggerFactory.CreateLogger<FoodService>());
            var statisticsService = new StatisticsService(userInterface, calorieCalculator, foodRepository);
            var foodDiaryManager = new FoodDiaryManager(foodService, statisticsService, userInterface);

            try
            {
                // Инициализация данных пользователя
                User user = await userDataInitializer.InitializeUserDataAsync();

                // Запуск основного цикла
                await foodDiaryManager.RunAsync(user);
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка в программе: {ex.Message}");
            }
        }
    }
}
