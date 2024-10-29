using Food_Diary.Models;
using Дневник_Питания.Meal;

namespace Дневник_Питания.Statistics
{
    public class WeeklyStatistics
    {
        public void ShowWeeklyStatistics(List<FoodEntry> foods)
        {
            DateTime weekStart = DateTime.Now.Date.AddDays(-7);
            var weeklyFoods = foods.Where(f => f.Date.Date >= weekStart).ToList();

            Console.WriteLine("Статистика за неделю:");
            if (weeklyFoods.Any())
            {
                foreach (var food in weeklyFoods)
                {
                    Console.WriteLine($"{food.Name} - {food.Calories} ккал");
                }
            }
            else
            {
                Console.WriteLine("Нет записей о питании за последнюю неделю.");
            }
        }
    }
}