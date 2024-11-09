using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserInterface _userInterface;
        private readonly ICalorieCalculator _calorieCalculator;
        private readonly IFoodRepository _foodRepository;

        public StatisticsService(IUserInterface userInterface, ICalorieCalculator calorieCalculator, IFoodRepository foodRepository)
        {
            _userInterface = userInterface;
            _calorieCalculator = calorieCalculator;
            _foodRepository = foodRepository;
        }

        public async Task ShowStatisticsAsync(User user)
        {
            string choice;
            while (true)
            {
                await _userInterface.WriteMessageAsync("\nВыберите период для отображения статистики:");
                await _userInterface.WriteMessageAsync("1. За день");
                await _userInterface.WriteMessageAsync("2. За неделю");
                await _userInterface.WriteMessageAsync("3. За месяц");
                await _userInterface.WriteMessageAsync("Ваш выбор (1-3): ");

                choice = await _userInterface.ReadInputAsync();

                if (choice == "1" || choice == "2" || choice == "3")
                {
                    break;
                }
                else
                {
                    await _userInterface.WriteMessageAsync("Ошибка! Пожалуйста, введите число от 1 до 3.");
                }
            }

            DateTime now = DateTime.Now;
            DateTime startDate = choice switch
            {
                "1" => now.Date,
                "2" => now.Date.AddDays(-7),
                "3" => now.Date.AddMonths(-1),
                _ => now.Date
            };

            // Загружаем данные о продуктах каждый раз при запросе статистики
            var foods = await _foodRepository.GetAllFoodsAsync();
            var foodsInPeriod = foods.Where(f => f.Date >= startDate && f.Date <= now).ToList();

            if (!foodsInPeriod.Any())
            {
                await _userInterface.WriteMessageAsync("Нет данных для выбранного периода.");
                return;
            }

            var mealsGrouped = foodsInPeriod.GroupBy(f => f.MealTime)
                                            .ToDictionary(g => g.Key, g => g.ToList());

            double totalCaloriesConsumed = 0;

            await _userInterface.WriteMessageAsync("\nСтатистика потребленных калорий:");

            foreach (var meal in mealsGrouped)
            {
                await _userInterface.WriteMessageAsync($"\n{meal.Key.First().ToString().ToUpper() + meal.Key.Substring(1)}:");

                double mealCalories = 0;

                foreach (var food in meal.Value)
                {
                    await _userInterface.WriteMessageAsync($"{food.Name} - {food.Calories} ккал");
                    mealCalories += food.Calories;
                }

                totalCaloriesConsumed += mealCalories;
                await _userInterface.WriteMessageAsync($"Всего на {meal.Key}: {mealCalories} ккал");
            }

            await _userInterface.WriteMessageAsync($"\nОбщая статистика за выбранный период времени:");
            await _userInterface.WriteMessageAsync($"Потреблено калорий: {totalCaloriesConsumed} ккал");

            double dailyCaloriesBurned = _calorieCalculator.CalculateTotalCalories(user);
            double totalCaloriesBurned = dailyCaloriesBurned * (now - startDate).Days;

            await _userInterface.WriteMessageAsync($"Сожженные калории по расчету BMR: {totalCaloriesBurned} ккал");

            if (totalCaloriesConsumed <= totalCaloriesBurned && totalCaloriesConsumed <= user.TargetCalories)
            {
                await _userInterface.WriteMessageAsync("Поздравляем! Вы достигли ваших целевых показателей калорийности!");
            }
            else
            {
                await _userInterface.WriteMessageAsync("Целевая калорийность не достигнута. Не расстраивайтесь! Продолжайте стараться, и вы достигнете своей цели!");
            }
        }
    }
}
