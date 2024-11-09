using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces.Services;

public interface ICalorieCalculator
{
    double CalculateBMR(User user);
    double CalculateTotalCalories(User user);
}