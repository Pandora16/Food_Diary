using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services
{
    public class FoodDiaryManager : IFoodDiaryManager
    {
        private readonly IUserInputManager _inputManager;
        private readonly ICalorieCalculator _calorieCalculator;
        private readonly IUserInterface _userInterface;

        public List<Food> Foods { get; set; } = new List<Food>();

        public FoodDiaryManager(IUserInputManager inputManager, ICalorieCalculator calorieCalculator, IUserInterface userInterface)
        {
            _inputManager = inputManager;
            _calorieCalculator = calorieCalculator;
            _userInterface = userInterface;
        }

        public async Task AddFoodAsync()
        {
            Food food = new Food();

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

            food.Calories = _inputManager.GetPositiveDouble("Введите калорийность продукта (в ккал): ");
            food.Proteins = _inputManager.GetPositiveDouble("Введите количество белков (в г): ");
            food.Fats = _inputManager.GetPositiveDouble("Введите количество жиров (в г): ");
            food.Carbohydrates = _inputManager.GetPositiveDouble("Введите количество углеводов (в г): ");
            food.MealTime = await _inputManager.GetMealTimeAsync();
            food.Date = DateTime.Now;

            Foods.Add(food);
            await _userInterface.WriteMessageAsync("Продукт успешно добавлен!");
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

            // Отбираем продукты, которые попадают в выбранный период
            var foodsInPeriod = Foods.Where(f => f.Date >= startDate && f.Date <= now).ToList();

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

            // Расчет сожженных калорий на основе BMR пользователя и выбранного периода
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
