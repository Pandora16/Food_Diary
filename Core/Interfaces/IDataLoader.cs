using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IDataLoader
    {
        (User, List<Food>) LoadData(string filePath);
    }
}