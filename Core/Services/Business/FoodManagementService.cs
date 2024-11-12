using System.Threading.Tasks;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Interfaces.UI;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business
{
    public class FoodManagementService : IFoodManagementService
    {
        private readonly IUserInputManager _inputManager;
        private readonly IUserInterface _userInterface;
        private readonly IFoodRepository _foodRepository;

        public FoodManagementService(IFoodRepository foodRepository, IUserInputManager inputManager, IUserInterface userInterface)
        {
            _foodRepository = foodRepository;
            _inputManager = inputManager;
            _userInterface = userInterface;
        }

        public async Task AddFoodAsync()
        {
            Food food = new Food();

            // Сбор данных о продукте от пользователя
            while (true)
            {
                await _userInterface.WriteMessageAsync("Введите название продукта: ");
                food.Name = await _userInterface.ReadInputAsync();

                if (string.IsNullOrWhiteSpace(food.Name))
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Название продукта не может быть пустым.");
                }
                else if (!food.Name.Any(char.IsLetter))
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Название продукта не должно состоять только из цифр.");
                }
                else
                {
                    break;
                }
            }

            // Сбор информации о питательной ценности продукта
            food.Calories = await _inputManager.GetPositiveDoubleAsync("Введите калорийность продукта (в ккал): ");
            food.Proteins = await _inputManager.GetPositiveDoubleAsync("Введите количество белков (в г): ");
            food.Fats = await _inputManager.GetPositiveDoubleAsync("Введите количество жиров (в г): ");
            food.Carbohydrates = await _inputManager.GetPositiveDoubleAsync("Введите количество углеводов (в г): ");
            food.MealTime = await _inputManager.GetMealTimeAsync();
            food.Date = DateTime.Now;

            // Передаем сохранение продукта в репозиторий
            await _foodRepository.SaveFoodAsync(food);
            await _userInterface.WriteMessageAsync("Продукт успешно добавлен!");
        }
    }
}