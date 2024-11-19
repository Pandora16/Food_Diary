// Класс для метода расширения, кот-рый позволяет преобразовать значение перечисления MealTime в строку на русском языке:завтрак, обед, ужин
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Services.Business
{
    public static class MealTimeExtensions
    {
        public static string ToLocalizedString(this MealTime mealTime)
        {
            return mealTime switch
            {
                MealTime.Breakfast => "завтрак",
                MealTime.Lunch => "обед",
                MealTime.Dinner => "ужин",
                _ => throw new ArgumentOutOfRangeException(nameof(mealTime), mealTime, null)
            };
        }
    }
}