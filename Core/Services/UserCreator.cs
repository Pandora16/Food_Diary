using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;
using Microsoft.Extensions.Logging;
using Дневник_Питания.Core.Interfaces.UI;

namespace Дневник_Питания.Core.Services
{
    public class UserCreator : IUserCreator
    {
        private readonly IUserInputManager _inputManager;
        private readonly ICalorieCalculator _calorieCalculator;
        private readonly ILogger<UserCreator> _logger;

        public UserCreator(IUserInputManager inputManager, ICalorieCalculator calorieCalculator, ILogger<UserCreator> logger)
        {
            _inputManager = inputManager;
            _calorieCalculator = calorieCalculator;
            _logger = logger;
        }

        public async Task<User> CreateNewUserAsync()
        {
            Console.WriteLine("Добро пожаловать в электронный дневник питания!");
            User user = new User
            {
                Height = await _inputManager.GetPositiveIntegerAsync("Введите ваш рост (в см): "),
                Weight = await _inputManager.GetPositiveIntegerAsync("Введите ваш вес (в кг): "),
                Age = await _inputManager.GetPositiveIntegerAsync("Введите ваш возраст (в годах): "),
                Gender = await _inputManager.GetGenderAsync(),
                ActivityLevel = await _inputManager.GetActivityLevelAsync()
            };
            user.BMR = _calorieCalculator.CalculateBMR(user);
            user.TargetCalories = await _inputManager.GetPositiveIntegerAsync("Введите вашу целевую калорийность (в ккал): ");
            _logger.LogInformation("Новый пользователь успешно создан.");
            return user;
        }
    }
}