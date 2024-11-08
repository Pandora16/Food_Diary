using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Дневник_Питания.Core.Repositories;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            // Конфигурация DI контейнера
            var serviceProvider = ConfigureServices();
            
            // Разрешение зависимостей через DI контейнер
            var userInterface = serviceProvider.GetRequiredService<IUserInterface>();
            var inputManager = serviceProvider.GetRequiredService<IUserInputManager>();
            var calorieCalculator = serviceProvider.GetRequiredService<ICalorieCalculator>();
            var foodRepository = serviceProvider.GetRequiredService<IFoodRepository>();
            var foodService = serviceProvider.GetRequiredService<IFoodService>();
            var statisticsService = serviceProvider.GetRequiredService<IStatisticsService>();

            string filePath = "foodDiary.json";
            User user;

            if (File.Exists(filePath))
            {
                string response;
                while (true)
                {
                    Console.WriteLine("Хотите очистить данные и начать заново? (да/нет)");
                    response = Console.ReadLine()?.ToLower();
                    if (response == "да")
                    {
                        File.Delete(filePath); // Удаление старого файла
                        Console.WriteLine("Данные очищены.");
                        user = await CreateNewUser(inputManager, calorieCalculator); // Создание нового пользователя
                        break; // Выход из цикла
                    }
                    else if (response == "нет")
                    {
                        Console.WriteLine("Загрузка данных из файла...");
                        user = await foodRepository.LoadUserAsync();
                        if (user == null)
                        {
                            Console.WriteLine("Ошибка при загрузке данных.");
                            return;
                        }
                        break; // Выход из цикла
                    }
                    else
                    {
                        Console.WriteLine("Ошибка! Пожалуйста, введите 'да' или 'нет'.");
                    }
                }
            }
            else
            {
                // Если файла нет
                user = await CreateNewUser(inputManager, calorieCalculator);
            }

            // Переход в основной цикл приложения
            await MainLoop(user, foodService, statisticsService);
        }

        // Метод для настройки DI контейнера
        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Регистрируем интерфейсы и их реализации
            serviceCollection.AddSingleton<IUserInputManager, UserInputManager>();
            serviceCollection.AddSingleton<IUserInterface, ConsoleUserInterface>();
            serviceCollection.AddSingleton<ICalorieCalculator, CalorieCalculator>();
            serviceCollection.AddSingleton<IFoodRepository>(provider => new FoodRepository("foodDiary.json"));
            serviceCollection.AddSingleton<IFoodService, FoodService>();
            serviceCollection.AddSingleton<IStatisticsService, StatisticsService>();

            return serviceCollection.BuildServiceProvider();
        }

        // Метод для создания нового пользователя
        private static async Task<User> CreateNewUser(IUserInputManager inputManager, ICalorieCalculator calorieCalculator)
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
            return user;
        }

        // Метод для основного цикла программы
        private static async Task MainLoop(User user, IFoodService foodService, IStatisticsService statisticsService)
        {
            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить продукт");
                Console.WriteLine("2. Показать статистику");
                Console.WriteLine("3. Выход");
                Console.Write("Ваш выбор (1-3): ");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        await foodService.AddFoodAsync();
                        break;
                    case "2":
                        await statisticsService.ShowStatisticsAsync(user);
                        break;
                    case "3":
                        await foodService.SaveAllDataAsync(user);  // Сохранение данных без filePath
                        Console.WriteLine("До свидания! Хорошего дня!");
                        return;
                    default:
                        Console.WriteLine("Ошибка! Введите корректный номер действия (1-3).");
                        break;
                }
            }
        }
    }
}
