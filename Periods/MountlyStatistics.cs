using Food_Diary.Models;
using Дневник_Питания.Meal;

namespace Дневник_Питания.Statistics
{
    public class MonthlyStatistics
    {
        public void ShowMonthlyStatistics(List<FoodEntry> foods)
        {
            DateTime monthStart = DateTime.Now.Date.AddDays(-30);
            var monthlyFoods = foods.Where(f => f.Date.Date >= monthStart).ToList();

            Console.WriteLine("Статистика за месяц:");
            if (monthlyFoods.Any())
            {
                foreach (var food in monthlyFoods)
                {
                    Console.WriteLine($"{food.Name} - {food.Calories} ккал");
                }
            }
            else
            {
                Console.WriteLine("Нет записей о питании за последний месяц.");
            }
        }
    }
}