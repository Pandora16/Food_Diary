using Дневник_Питания.Core.Models;

public interface ICalorieCalculator
{
    double CalculateBMR(User user);
    double CalculateTotalCalories(User user);
}