using Дневник_Питания.Core.Models;

namespace Дневник_Питания.Core.Interfaces
{
    public interface IDataSaver
    {
        void SaveData(string filePath, User user, List<Food> foods);
    }
}