using Дневник_Питания.Core.Interfaces;

namespace Дневник_Питания.Core.Services;

public class UserInputManager : IUserInputManager
{
    // Метод для ввода целого положительного числа ( рост и вес )
    public static int GetPositiveInteger(string message)
    {
        int value;
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Ошибка! Введите целое положительное число.");
        }
    }

    // Метод для ввода положительного вещественного числа ( ккал, жиры, белки, углеводы )
    public double GetPositiveDouble(string message)
    {
        double value;
        while (true)
        {
            Console.Write(message);
            if (double.TryParse(Console.ReadLine(), out value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("Ошибка! Введите положительное число.");
        }
    }
    
    // Метод для ввода времени приема пищи с проверкой
        public string GetMealTime()
        {
            while (true)
            {
                Console.Write("Введите время приема пищи (завтрак, обед, ужин): ");
                string mealTime = Console.ReadLine().ToLower();
                if (mealTime == "завтрак" || mealTime == "обед" || mealTime == "ужин")
                {
                    return mealTime;
                }
                Console.WriteLine("Ошибка! Введите одно из значений: завтрак, обед, ужин.");
            }
        }

        public static string GetGender()
        {
            while (true)
            {
                Console.Write("Введите ваш пол (м/ж): ");
                string gender = Console.ReadLine().ToLower();
                if (gender == "м" || gender == "ж")
                {
                    return gender;
                }
                Console.WriteLine("Ошибка! Введите 'м' или 'ж'.");
            }
        }

        public static string GetActivityLevel()
        {
            Console.WriteLine("Выберите уровень физической активности:");
            Console.WriteLine("1. Сидячий образ жизни (нет физических упражнений)");
            Console.WriteLine("2. Легкие упражнения (легкие физические нагрузки 1-3 раза в неделю)");
            Console.WriteLine("3. Умеренные упражнения (умеренные физические нагрузки 3-5 раз в неделю)");
            Console.WriteLine("4. Активный образ жизни (интенсивные физические нагрузки 6-7 раз в неделю)");
            Console.WriteLine("5. Очень активный (очень интенсивные физические нагрузки и физическая работа)");

            string choice;
            while (true)
            {
                Console.Write("Ваш выбор (1-5): ");
                choice = Console.ReadLine();
                if (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5")
                {
                    return choice;
                }
                Console.WriteLine("Ошибка! Пожалуйста, введите число от 1 до 5.");
            }
        }

}