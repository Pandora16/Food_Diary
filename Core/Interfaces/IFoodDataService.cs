using Дневник_Питания.Core.Models;

    public interface IFoodDataService
    {
        Task SaveAllDataAsync(User user);
}