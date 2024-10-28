using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

namespace Дневник_Питания.Statistics
{
    public class StatisticsManager
    {
        public void ShowStatistics(User user, List<Food> foods)
        {
            double totalCalories = foods.Sum(f => f.Calories);

            Console.WriteLine($"Потреблено калорий: {totalCalories} ккал");

            // Логика для расчёта сожжённых калорий
            double totalCaloriesBurned = CalorieCalculator.CalculateBMR(user);

            if (totalCalories <= totalCaloriesBurned)
            {
                Console.WriteLine("Вы достигли своей цели по калориям!");
            }
            else
            {
                Console.WriteLine("Вам не удалось достичь цели. Попробуйте ещё раз!");
            }
        }
    }
}