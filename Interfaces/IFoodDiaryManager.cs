using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces;

public interface IFoodDiaryManager
{
    Task AddFoodAsync();
    Task ShowStatisticsAsync(User user);
}