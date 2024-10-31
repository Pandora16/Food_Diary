namespace Дневник_Питания.Core.Interfaces;

public interface IUserInputManager
{
    double GetPositiveDouble(string prompt);
    string GetMealTime();
}