
using System.Threading.Tasks;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IUserInputManager
    {
        Task<string> GetMealTimeAsync();
        Task<int> GetPositiveIntegerAsync(string message);
        Task<double> GetPositiveDoubleAsync(string message);
        Task<string> GetGenderAsync();
        Task<string> GetActivityLevelAsync();
    }
}
