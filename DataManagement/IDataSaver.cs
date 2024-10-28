using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

namespace Дневник_Питания.DataManagement
{
    public interface IDataSaver
    {
        void SaveData(string filePath, User user, List<Food> foods);
    }
}