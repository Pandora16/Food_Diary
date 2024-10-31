using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces;

public interface IFoodDiaryManager
{
    void AddFood();
    void ShowStatistics(User user);
}