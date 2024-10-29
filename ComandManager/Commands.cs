using Дневник_Питания.Interface;
using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

namespace Дневник_Питания.ComandManager;


    public class AddFoodCommand : ICommand
    {
        public async Task ExecuteAsync(string message, IUserInterface userInterface)
        {
            await userInterface.WriteMessageAsync("Введите данные продукта для добавления.");
            // Логика добавления продукта
        }
    }

    public class ShowStatisticsCommand : ICommand
    {
        public async Task ExecuteAsync(string message, IUserInterface userInterface)
        {
            await userInterface.WriteMessageAsync("Отображение статистики.");
            // Логика отображения статистики
            
        }
    }

    public class ExitCommand : ICommand
    {
        private readonly IDataManager _dataManager;
        private readonly User _user;
        private readonly List<Food> _foods;

        public ExitCommand(IDataManager dataManager, User user, List<Food> foods)
        {
            _dataManager = dataManager;
            _user = user;
            _foods = foods;
        }

        public async Task ExecuteAsync(string message, IUserInterface userInterface)
        {
            await _dataManager.SaveDataAsync("foodDiary.json", _user, _foods);
            await userInterface.WriteMessageAsync("До свидания! Хорошего дня!");
        }
    }
