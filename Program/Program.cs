using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "foodDiary.json";
            IUserInterface userInterface = new ConsoleUserInterface();
            IUserInputManager inputManager = new UserInputManager(userInterface);
            ICalorieCalculator calorieCalculator = new CalorieCalculator();

            IFoodRepository foodRepository = new FoodRepository(filePath);  // Инициализация с путем внутри репозитория
            IFoodService foodService = new FoodService(foodRepository, inputManager, userInterface);

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
            
            IStatisticsService statisticsService = new StatisticsService(userInterface, calorieCalculator, foodRepository);

            await MainLoop(user, foodService, statisticsService);
        }

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
