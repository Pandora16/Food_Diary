using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.UI;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business
{
    public class UserInputManager : IUserInputManager
    {
        private readonly IUserInterface _userInterface;
        private IUserInputManager _userInputManagerImplementation;

        public UserInputManager(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        // Асинхронный метод для ввода времени приема пищи с проверкой

        public async Task<MealTime> GetMealTimeAsync()
        {
            while (true)
            {
                await _userInterface.WriteMessageAsync("Введите время приема пищи (завтрак, обед, ужин): ");
                string input = (await _userInterface.ReadInputAsync()).ToLower();

                switch (input)
                {
                    case "завтрак":
                        return MealTime.Breakfast;
                    case "обед":
                        return MealTime.Lunch;
                    case "ужин":
                        return MealTime.Dinner;
                    default:
                        await _userInterface.WriteMessageAsync("Ошибка! Введите одно из значений: завтрак, обед, ужин.");
                        break;
                }
            }
        }


        // Асинхронный метод для ввода положительного целого числа (например, для роста и веса)
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

        // Асинхронный метод для ввода положительного вещественного числа (ккал, жиры, белки, углеводы)
        public async Task<double> GetPositiveDoubleAsync(string message)
        {
            double value;
            while (true)
            {
                await _userInterface.WriteMessageAsync(message);
                if (double.TryParse(await _userInterface.ReadInputAsync(), out value) && value >= 0)
                {
                    return value;
                }
                await _userInterface.WriteMessageAsync("Ошибка! Введите положительное число.");
            }
        }

        // Асинхронный метод для ввода пола
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

        // Асинхронный метод для выбора уровня активности
        public async Task<string> GetActivityLevelAsync()
        {
            await _userInterface.WriteMessageAsync("Выберите уровень физической активности:");
            await _userInterface.WriteMessageAsync("1. Сидячий образ жизни (нет физических упражнений)");
            await _userInterface.WriteMessageAsync("2. Легкие упражнения (легкие физические нагрузки 1-3 раза в неделю)");
            await _userInterface.WriteMessageAsync("3. Умеренные упражнения (умеренные физические нагрузки 3-5 раз в неделю)");
            await _userInterface.WriteMessageAsync("4. Активный образ жизни (интенсивные физические нагрузки 6-7 раз в неделю)");
            await _userInterface.WriteMessageAsync("5. Очень активный (очень интенсивные физические нагрузки и физическая работа)");

            while (true)
            {
                await _userInterface.WriteMessageAsync("Ваш выбор (1-5): ");
                string choice = await _userInterface.ReadInputAsync();

                if (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5")
                {
                    return choice;
                }
                await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 5.");
            }
        }
    }
}
