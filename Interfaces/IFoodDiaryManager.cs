namespace Дневник_Питания.Interfaces
{
    public interface IFoodDiaryManager
    {
        Task AddFoodAsync(string name, int calories);
        Task RemoveFoodAsync(string name);
        Task ShowSummaryAsync();
    }
}