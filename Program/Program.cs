using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services;
using Дневник_Питания.Data;
using static Дневник_Питания.Core.Services.UserInputManager;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string filePath = "foodDiary.json";
            User user;

            // Создание необходимых интерфейсов
            IUserInputManager inputManager = new UserInputManager();
            ICalorieCalculator calorieCalculator = new CalorieCalculator();
            IUserInterface userInterface = new ConsoleUserInterface();

            // Создаем экземпляр FoodDiaryManager с необходимыми зависимостями
            FoodDiaryManager foodDiaryManager = new FoodDiaryManager(inputManager, calorieCalculator, userInterface);
            FileManager fileManager = new FileManager();

            // Удаление загруженных данных, если они есть, по выбору пользователя 
            if (File.Exists(filePath))
            {
                string response = GetUserConfirmation("Хотите очистить данные и начать заново? (да/нет)");

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
                (user, List<Food> foods) = fileManager.LoadData(filePath); // Используем экземпляр fileManager
                if (user == null)
                {
                    Console.WriteLine("Ошибка при загрузке данных.");
                    return;
                }
                foodDiaryManager.Foods.AddRange(foods); // Добавляем загруженные продукты в foodDiary
            }
            else
            {
                user = CreateNewUser(calorieCalculator); // Передаем экземпляр calorieCalculator
                ;
            }

            // Экземпляр FileManager передаётся в MainLoop, чтобы можно было использовать его методы SaveData и LoadData
            MainLoop(user, foodDiaryManager, fileManager, filePath);
        }

        private static string GetUserConfirmation(string message)
        {
            string response;
            while (true)
            {
                Console.WriteLine(message);
                response = Console.ReadLine().ToLower();

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

        private static User CreateNewUser(ICalorieCalculator calorieCalculator)
        {
            User user = new User();
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");
            user.Height = GetPositiveInteger("Введите ваш рост (в см): ");
            user.Weight = GetPositiveInteger("Введите ваш вес (в кг): ");
            user.Age = GetPositiveInteger("Введите ваш возраст (в годах): ");
            user.Gender = GetGender();
            user.ActivityLevel = GetActivityLevel();
            user.BMR = calorieCalculator.CalculateBMR(user); // Вызов метода через экземпляр
            user.TargetCalories = GetPositiveInteger("Введите вашу целевую калорийность (в ккал): ");
            return user;
        }


        private static void MainLoop(User user, FoodDiaryManager foodDiaryManager, FileManager fileManager, string filePath)
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
                    foodDiaryManager.AddFood(); // Используем метод AddFood класса FoodDiary
                }
                else if (action == "2")
                {
                    foodDiaryManager.ShowStatistics(user); // Используем метод ShowStatistics класса FoodDiary
                }
                else if (action == "3")
                {
                    fileManager.SaveData(filePath, user, foodDiaryManager.Foods); // Сохраняем список продуктов через экземпляр fileManager
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
