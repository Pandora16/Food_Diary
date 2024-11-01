using Дневник_Питания.Models;
using Дневник_Питания.UserManagment;

namespace Дневник_Питания;

public class CalorieCalculator
{
    public static double CalculateBMR(User user)
    {
        if (user.Gender == "м")
        {
            // Формула для мужчин
            return 10 * user.Weight + 6.25 * user.Height - 5 * user.Age + 5;
        }
        else
        {
            // Формула для женщин
            return 10 * user.Weight + 6.25 * user.Height - 5 * user.Age - 161;
        }
    }

    public static double CalculateTotalCalories(User user)
    {
        double activityFactor;
        if (user.ActivityLevel == "1") activityFactor = 1.2;
        else if (user.ActivityLevel == "2") activityFactor = 1.375;
        else if (user.ActivityLevel == "3") activityFactor = 1.55;
        else if (user.ActivityLevel == "4") activityFactor = 1.725;
        else activityFactor = 1.9;

        return user.BMR * activityFactor;
    } 
}