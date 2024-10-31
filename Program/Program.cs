using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using Дневник_Питания.Data;
using System.Threading.Tasks;
using Дневник_Питания.Interfaces;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "foodDiary.json";
            User user;

            // Создание необходимых интерфейсов
            IUserInterface userInterface = new ConsoleUserInterface();
            IUserInputManager inputManager = new UserInputManager(userInterface);
            ICalorieCalculator calorieCalculator = new CalorieCalculator();

            // Создаем экземпляр FoodDiaryRepository для работы с данными
            IFoodDiaryRepository foodDiaryRepository = new FoodDiaryRepository();
            FoodDiaryManager foodDiaryManager = new FoodDiaryManager(inputManager, calorieCalculator, userInterface);

            // Удаление загруженных данных, если они есть, по выбору пользователя 
            if (File.Exists(filePath))
            {
                string response = await GetUserConfirmation("Хотите очистить данные и начать заново? (да/нет)");

                if (response == "да")
                {
                    File.Delete(filePath);
                    Console.WriteLine("Данные очищены.");
                }
            }

            // Загрузка данных из файла, если они есть
            if (File.Exists(filePath))
            {
                Console.WriteLine("Загрузка данных из файла...");
                (user, var foods) = await foodDiaryRepository.LoadDataAsync(filePath);
                if (user == null)
                {
                    Console.WriteLine("Ошибка при загрузке данных.");
                    return;
                }
                foodDiaryManager.Foods.AddRange(foods);
            }
            else
            {
                user = await CreateNewUser(inputManager, calorieCalculator);
            }

            // Запуск основного цикла с передачей репозитория для сохранения
            await MainLoop(user, foodDiaryManager, foodDiaryRepository, filePath);
        }

        private static async Task<string> GetUserConfirmation(string message)
        {
            string response;
            while (true)
            {
                Console.WriteLine(message);
                response = (await Console.In.ReadLineAsync()).ToLower();

                if (response == "да" || response == "нет")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 'да' или 'нет'.");
                }
            }
            return response;
        }

        private static async Task<User> CreateNewUser(IUserInputManager inputManager, ICalorieCalculator calorieCalculator)
        {
            User user = new User();
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");
            user.Height = await inputManager.GetPositiveIntegerAsync("Введите ваш рост (в см): ");
            user.Weight = await inputManager.GetPositiveIntegerAsync("Введите ваш вес (в кг): ");
            user.Age = await inputManager.GetPositiveIntegerAsync("Введите ваш возраст (в годах): ");
            user.Gender = await inputManager.GetGenderAsync();
            user.ActivityLevel = await inputManager.GetActivityLevelAsync();
            user.BMR = calorieCalculator.CalculateBMR(user);
            user.TargetCalories = await inputManager.GetPositiveIntegerAsync("Введите вашу целевую калорийность (в ккал): ");
            return user;
        }

        private static async Task MainLoop(User user, FoodDiaryManager foodDiaryManager, IFoodDiaryRepository foodDiaryRepository, string filePath)
        {
            while (true)
            {
                Console.WriteLine(
                    "\nВы можете добавлять продукты, учитывая их калорийность и пищевую ценность для расчёта сожжённых калорий.");
                Console.WriteLine("1. Добавить продукт");
                Console.WriteLine("2. Просмотреть статистику");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите действие (1-3): ");
                string action = Console.ReadLine();

                if (action == "1")
                {
                    await foodDiaryManager.AddFoodAsync();
                }
                else if (action == "2")
                {
                    await foodDiaryManager.ShowStatisticsAsync(user);
                }
                else if (action == "3")
                {
                    await foodDiaryRepository.SaveDataAsync(filePath, user, foodDiaryManager.Foods);
                    Console.WriteLine("До свидания! Хорошего дня!");
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите корректный номер действия (1-3).");
                }
            }
        }
    }
}
