using Дневник_Питания.DataManagement;
using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;
using static Дневник_Питания.UserManagment.UserInputManager;
using static Дневник_Питания.CalorieCalculator;

namespace Дневник_Питания.Program
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string filePath = "foodDiary.json";
            User user;
            FoodDiary foodDiary = new FoodDiary();
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
                // AddRange - метод класса List<T>, который добавляет несколько элементов из другой коллекции в текущий список.
                // В данном случае, мы передаём foods, чтобы добавить все загруженные продукты в foodDiary. Foods сразу, а не по одному.
                foodDiary.Foods.AddRange(foods); // Добавляем загруженные продукты в foodDiary
            }
            else
            {
                user = CreateNewUser();
            }
            // Экземпляр FileManager передаётся в MainLoop, чтобы можно было использовать его методы SaveData и LoadData
            MainLoop(user, foodDiary, fileManager, filePath);
        }

        private static string GetUserConfirmation(string message)
        {
            string response;
            while (true)
            {
                Console.WriteLine(message);
                // ToLower преобразует строку, введённую пользователем, в нижний регистр.
                // Это позволяет избежать ошибок при сравнении: если пользователь введёт "Да" или "ДА", мы всё равно получим "да".
                response = Console.ReadLine().ToLower();

                if (response == "да" || response == "нет")
                {
                    break; // Ввод корректен, выходим из цикла
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 'да' или 'нет'.");
                }
            }
            return response;
        }

        private static User CreateNewUser()
        {
            User user = new User();
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");
            user.Height = GetPositiveInteger("Введите ваш рост (в см): ");
            user.Weight = GetPositiveInteger("Введите ваш вес (в кг): ");
            user.Age = GetPositiveInteger("Введите ваш возраст (в годах): ");
            user.Gender = GetGender();
            user.ActivityLevel = GetActivityLevel();
            user.BMR = CalculateBMR(user);
            user.TargetCalories = GetPositiveInteger("Введите вашу целевую калорийность (в ккал): ");
            return user;
        }
        // Метод MainLoop представляет собой основной цикл приложения,
        // где происходит взаимодействие с пользователем.
        // Он позволяет пользователю добавлять продукты, просматривать статистику и выходить из приложения
        private static void MainLoop(User user, FoodDiary foodDiary, FileManager fileManager, string filePath) //
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

                // Проверка на корректный ввод
                if (action == "1")
                {
                    foodDiary.AddFood(); // Используем метод AddFood класса FoodDiary
                }
                else if (action == "2")
                {
                    foodDiary.ShowStatistics(user); // Используем метод ShowStatistics класса FoodDiary
                }
                else if (action == "3")
                {
                    fileManager.SaveData(filePath, user, foodDiary.Foods); // Сохраняем список продуктов через экземпляр fileManager
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