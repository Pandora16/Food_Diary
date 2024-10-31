using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddFood()
        {
            Food food = new Food();

            while (true)
            {
                _userInterface.WriteMessage("Введите название продукта: ");
                food.Name = _userInterface.ReadInput();

                if (string.IsNullOrWhiteSpace(food.Name))
                {
                    _userInterface.WriteMessage("Ошибка! Название продукта не может быть пустым.");
                }
                else if (!food.Name.Any(char.IsLetter))
                {
                    _userInterface.WriteMessage("Ошибка! Название продукта не должно состоять только из цифр.");
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
            food.MealTime = _inputManager.GetMealTime();
            food.Date = DateTime.Now;

            Foods.Add(food);
            _userInterface.WriteMessage("Продукт успешно добавлен!");
        }

        public void ShowStatistics(User user)
        {
            string choice;
            while (true)
            {
                _userInterface.WriteMessage("\nВыберите период для отображения статистики:");
                _userInterface.WriteMessage("1. За день");
                _userInterface.WriteMessage("2. За неделю");
                _userInterface.WriteMessage("3. За месяц");
                _userInterface.WriteMessage("Ваш выбор (1-3): ");

                choice = _userInterface.ReadInput();

                if (choice == "1" || choice == "2" || choice == "3")
                {
                    break;
                }
                else
                {
                    _userInterface.WriteMessage("Ошибка! Пожалуйста, введите число от 1 до 3.");
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
                _userInterface.WriteMessage("Нет данных для выбранного периода.");
                return;
            }

            var mealsGrouped = foodsInPeriod.GroupBy(f => f.MealTime)
                                            .ToDictionary(g => g.Key, g => g.ToList());

            double totalCaloriesConsumed = 0;

            _userInterface.WriteMessage("\nСтатистика потребленных калорий:");

            foreach (var meal in mealsGrouped)
            {
                _userInterface.WriteMessage($"\n{meal.Key.First().ToString().ToUpper() + meal.Key.Substring(1)}:");

                double mealCalories = 0;

                foreach (var food in meal.Value)
                {
                    _userInterface.WriteMessage($"{food.Name} - {food.Calories} ккал");
                    mealCalories += food.Calories;
                }

                totalCaloriesConsumed += mealCalories;
                _userInterface.WriteMessage($"Всего на {meal.Key}: {mealCalories} ккал");
            }

            _userInterface.WriteMessage($"\nОбщая статистика за выбранный период времени:");
            _userInterface.WriteMessage($"Потреблено калорий: {totalCaloriesConsumed} ккал");

            // Расчет сожженных калорий на основе BMR пользователя и выбранного периода
            double dailyCaloriesBurned = _calorieCalculator.CalculateTotalCalories(user);
            double totalCaloriesBurned = dailyCaloriesBurned * (now - startDate).Days;

            _userInterface.WriteMessage($"Сожженные калории по расчету BMR: {totalCaloriesBurned} ккал");

            if (totalCaloriesConsumed <= totalCaloriesBurned && totalCaloriesConsumed <= user.TargetCalories)
            {
                _userInterface.WriteMessage("Поздравляем! Вы достигли ваших целевых показателей калорийности!");
            }
            else
            {
                _userInterface.WriteMessage("Целевая калорийность не достигнута. Не расстраивайтесь! Продолжайте стараться, и вы достигнете своей цели!");
            }
        }
    }
}
