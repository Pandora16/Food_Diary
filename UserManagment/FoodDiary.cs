using Дневник_Питания.UserManagment;

namespace Дневник_Питания.Meal;

class FoodDiary
{
    public List<Food> Foods { get; set; } = new List<Food>();

    public void AddFood()
    {
        Food food = new Food();

        // Проверка, что название продукта не состоит только из цифр
        while (true)
        {
            Console.Write("Введите название продукта: ");
            food.Name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(food.Name))
            {
                Console.WriteLine("Ошибка! Название продукта не может быть пустым.");
            }
            else if (!food.Name.Any(char.IsLetter))
            {
                Console.WriteLine("Ошибка! Название продукта не должно состоять только из цифр.");
            }
            else
            {
                break;
            }
        }

        food.Calories = UserInputManager.GetPositiveDouble("Введите калорийность продукта (в kkaл): ");
        food.Proteins = UserInputManager.GetPositiveDouble("Введите количество белков (в г): ");
        food.Fats = UserInputManager.GetPositiveDouble("Введите количество жиров (в г): ");
        food.Carbohydrates = UserInputManager.GetPositiveDouble("Введите количество углеводов (в г): ");
        food.MealTime = UserInputManager.GetMealTime();

        Foods.Add(food);
        Console.WriteLine("Продукт успешно добавлен!");
    }


    public void ShowStatistics(User user)
    {
        string choice;
        while (true)
        {
            Console.WriteLine("\nВыберите период для отображения статистики:");
            Console.WriteLine("1. За день");
            Console.WriteLine("2. За неделю");
            Console.WriteLine("3. За месяц");
            Console.Write("Ваш выбор (1-3): ");

            choice = Console.ReadLine();

            if (choice == "1" || choice == "2" || choice == "3")
            {
                break; // Ввод корректен, выходим из цикла
            }
            else
            {
                Console.WriteLine("Ошибка! Пожалуйста, введите число от 1 до 3.");
            }
        }

        DateTime now = DateTime.Now;
        DateTime startDate;

        if (choice == "1")
        {
            startDate = now.Date; // Сегодня
        }
        else if (choice == "2")
        {
            startDate = now.Date.AddDays(-7); // Последние 7 дней
        }
        else
        {
            startDate = now.Date.AddMonths(-1); // Последний месяц
        }

        // Группируем продукты по приему пищи (завтрак, обед, ужин)
        var mealsGrouped = Foods.GroupBy(f => f.MealTime)
            .ToDictionary(g => g.Key, g => g.ToList());

        double totalCaloriesConsumed = 0;

        Console.WriteLine("\nСтатистика потребленных калорий:");

        foreach (var meal in mealsGrouped)
        {
            Console.WriteLine($"\n{meal.Key.First().ToString().ToUpper() + meal.Key.Substring(1)}:");

            double mealCalories = 0;

            // Выводим все продукты и калории для данного приёма пищи
            foreach (var food in meal.Value)
            {
                Console.WriteLine($"{food.Name} - {food.Calories} ккал");
                mealCalories += food.Calories;
            }

            totalCaloriesConsumed += mealCalories;
            Console.WriteLine($"Всего на {meal.Key}: {mealCalories} ккал");
        }

        // Общая сумма потребленных калорий
        Console.WriteLine($"\nОбщая статистика за выбранный период времени:");
        Console.WriteLine($"Потреблено калорий: {totalCaloriesConsumed} ккал");

        // Расчет сожженных калорий по BMR
        double totalCaloriesBurned = CalorieCalculator.CalculateTotalCalories(user) * (now - startDate).Days;

        Console.WriteLine($"Сожженные калории по расчету BMR: {totalCaloriesBurned} ккал");

        if (totalCaloriesConsumed <= totalCaloriesBurned && totalCaloriesConsumed <= user.TargetCalories)
        {
            Console.WriteLine("Поздравляем! Вы достигли ваших целевых показателей калорийности!");
        }
        else
        {
            Console.WriteLine(
                "Целевая калорийность не достигнута. Не расстраивайтесь! Продолжайте стараться, и вы достигнете своей цели!");
        }
    }
}