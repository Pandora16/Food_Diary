using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services
{
    public class FoodDiaryManager
    {
        private readonly IFoodService _foodService;
        private readonly IStatisticsService _statisticsService;
        private readonly IUserInterface _userInterface;

        public FoodDiaryManager(IFoodService foodService, IStatisticsService statisticsService, IUserInterface userInterface)
        {
            _foodService = foodService;
            _statisticsService = statisticsService;
            _userInterface = userInterface;
        }

        public async Task RunAsync(User user)
        {
            while (true)
            {
                await _userInterface.WriteMessageAsync("\nВыберите действие:");
                await _userInterface.WriteMessageAsync("1. Добавить продукт");
                await _userInterface.WriteMessageAsync("2. Показать статистику");
                await _userInterface.WriteMessageAsync("3. Выход");
                await _userInterface.WriteMessageAsync("Ваш выбор (1-3): ");

                string choice = await _userInterface.ReadInputAsync();

                switch (choice)
                {
                    case "1":
                        await _foodService.AddFoodAsync();
                        break;
                    case "2":
                        await _statisticsService.ShowStatisticsAsync(user);
                        break;
                    case "3":
                        await _userInterface.WriteMessageAsync("Завершение работы...");
                        return;
                    default:
                        await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3.");
                        break;
                }
            }
        }
    }
}