using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces.UI
{
    public interface IUserInputManager
    {
        Task<MealTime> GetMealTimeAsync();
        Task<int> GetPositiveIntegerAsync(string message);
        Task<double> GetPositiveDoubleAsync(string message);
        Task<string> GetGenderAsync();
        Task<string> GetActivityLevelAsync();
    }
}
