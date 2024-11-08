using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IFoodService
    {
        Task AddFoodAsync();
        Task SaveAllDataAsync(User user);
    }
}