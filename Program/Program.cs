using Microsoft.Extensions.Logging;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Interfaces.UI;
using Дневник_Питания.Core.Repositories;
using Дневник_Питания.Core.Services.Business;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            // Конфигурация DI контейнера
            var serviceProvider = ConfigureServices();

            // Получение логгера
            var logger = serviceProvider.GetRequiredService<ILogger>();
            logger.LogInformation("Программа запущена.");

            // Получение менеджера дневника из DI контейнера
            var foodDiaryManager = serviceProvider.GetRequiredService<FoodDiaryManager>();

            // Проверка и загрузка данных пользователя
            User user = await InitializeUserData(serviceProvider, logger);

            // Запуск основного цикла через FoodDiaryManager
            await foodDiaryManager.RunAsync(user);
        }

        // Метод для настройки DI контейнера
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Регистрация интерфейсов и реализаций
            serviceCollection.AddSingleton<IUserInputManager, UserInputManager>();
            serviceCollection.AddSingleton<IUserInterface, ConsoleUserInterface>();
            serviceCollection.AddSingleton<ICalorieCalculator, CalorieCalculator>();
            serviceCollection.AddSingleton<IFoodRepository>(provider => new FoodRepository("foodDiary.json", provider.GetRequiredService<ILogger<FoodRepository>>()));
            serviceCollection.AddSingleton<IFoodManagementService, FoodManagementService>();
            serviceCollection.AddSingleton<IFoodDataService, FoodDataService>();
            serviceCollection.AddSingleton<IFoodService, FoodService>();
            serviceCollection.AddSingleton<IStatisticsService, StatisticsService>();

            // Регистрация FoodDiaryManager как единого контроллера
            serviceCollection.AddSingleton<FoodDiaryManager>();

            // Добавление логгирования
            serviceCollection.AddLogging(configure => configure.AddConsole())
                .AddSingleton<ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("General"));


            return serviceCollection.BuildServiceProvider();
        }

        // Метод для проверки и загрузки данных пользователя
        private static async Task<User> InitializeUserData(IServiceProvider serviceProvider, ILogger logger)
        {
            var foodRepository = serviceProvider.GetRequiredService<IFoodRepository>();
            var inputManager = serviceProvider.GetRequiredService<IUserInputManager>();
            var calorieCalculator = serviceProvider.GetRequiredService<ICalorieCalculator>();
            string filePath = "foodDiary.json";

            if (File.Exists(filePath))
            {
                Console.WriteLine("Хотите очистить данные и начать заново? (да/нет)");
                string response = Console.ReadLine()?.ToLower();
                if (response == "да")
                {
                    File.Delete(filePath); // Удаление старого файла
                    logger.LogInformation("Данные очищены.");
                    return await CreateNewUser(inputManager, calorieCalculator, logger);
                }
                else if (response == "нет")
                {
                    logger.LogInformation("Загрузка данных из файла...");
                    var user = await foodRepository.LoadUserAsync();
                    if (user == null)
                    {
                        logger.LogError("Ошибка при загрузке данных.");
                        Environment.Exit(1); // Завершение работы в случае ошибки
                    }
                    return user;
                }
                else
                {
                    logger.LogWarning("Неверный ввод, повторите попытку.");
                    return await InitializeUserData(serviceProvider, logger); // Повторный вызов при ошибке ввода
                }
            }
            else
            {
                return await CreateNewUser(inputManager, calorieCalculator, logger);
            }
        }

        // Метод для создания нового пользователя
        private static async Task<User> CreateNewUser(IUserInputManager inputManager, ICalorieCalculator calorieCalculator, ILogger logger)
        {
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");
            User user = new User
            {
                Height = await inputManager.GetPositiveIntegerAsync("Введите ваш рост (в см): "),
                Weight = await inputManager.GetPositiveIntegerAsync("Введите ваш вес (в кг): "),
                Age = await inputManager.GetPositiveIntegerAsync("Введите ваш возраст (в годах): "),
                Gender = await inputManager.GetGenderAsync(),
                ActivityLevel = await inputManager.GetActivityLevelAsync()
            };
            user.BMR = calorieCalculator.CalculateBMR(user);
            user.TargetCalories = await inputManager.GetPositiveIntegerAsync("Введите вашу целевую калорийность (в ккал): ");
            logger.LogInformation("Новый пользователь успешно создан.");
            return user;
        }
    }
}
