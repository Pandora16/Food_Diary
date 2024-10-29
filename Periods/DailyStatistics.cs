using Дневник_Питания.Meal;

namespace Дневник_Питания.Statistics
{
    public class DailyStatistics
    {
        public void ShowDailyStatistics(List<Food> foods)
        {
            DateTime today = DateTime.Now.Date;
            var dailyFoods = foods.Where(f => f.Date.Date == today).ToList();

            Console.WriteLine("Статистика за день:");
            if (dailyFoods.Any())
            {
                foreach (var food in dailyFoods)
                {
                    Console.WriteLine($"{food.Name} - {food.Calories} ккал");
                }
            }
            else
            {
                Console.WriteLine("Нет записей о питании за сегодняшний день.");
            }
        }
    }
}