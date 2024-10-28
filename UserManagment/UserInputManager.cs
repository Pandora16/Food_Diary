namespace Дневник_Питания.UserManagment;
using Дневник_Питания.Consol;

public class UserInputManager
{
    private readonly IUserInterface _userInterface;

    // Передаем объект IUserInterface через конструктор
    public UserInputManager(IUserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    public async Task<int> GetPositiveIntegerAsync(string message)
    {
        int value;
        while (true)
        {
            await _userInterface.WriteMessageAsync(message);
            if (int.TryParse(await _userInterface.ReadInputAsync(), out value) && value > 0)
            {
                return value;
            }
            await _userInterface.WriteMessageAsync("Ошибка! Введите целое положительное число.");
        }
    }

    public static double GetPositiveDouble(string message)
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

    public static string GetMealTime()
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

    public async Task<string> GetGenderAsync()
    {
        while (true)
        {
            await _userInterface.WriteMessageAsync("Введите ваш пол (м/ж): ");
            string gender = (await _userInterface.ReadInputAsync()).ToLower();
            if (gender == "м" || gender == "ж")
            {
                return gender;
            }
            await _userInterface.WriteMessageAsync("Ошибка! Введите 'м' или 'ж'.");
        }
    }

    public async Task<string> GetActivityLevelAsync()
    {
        await _userInterface.WriteMessageAsync("Выберите уровень физической активности:\n1. Сидячий образ жизни\n2. Легкие упражнения\n3. Умеренные упражнения\n4. Активный образ жизни\n5. Очень активный");
    
        while (true)
        {
            await _userInterface.WriteMessageAsync("Ваш выбор (1-5): ");
            string choice = await _userInterface.ReadInputAsync();
            if (new[] { "1", "2", "3", "4", "5" }.Contains(choice))
            {
                return choice;
            }
            await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 5.");
        }
    }
}
