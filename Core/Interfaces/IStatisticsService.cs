using System.Threading.Tasks;
using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IStatisticsService
    {
        Task ShowStatisticsAsync(User user);
    }
}