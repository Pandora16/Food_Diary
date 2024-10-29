using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

namespace Дневник_Питания.DataManagement
{
    public interface IDataLoader
    {
        (User, List<Food>) LoadData(string filePath);
    }
}