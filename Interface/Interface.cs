using Дневник_Питания.ComandManager;
using Дневник_Питания.Consol;
using Дневник_Питания.Meal;
using Дневник_Питания.UserManagment;

internal static class Interface
{
    private static IUserInterface _userInterface;
    private static IDataManager _dataManager;
    private static Dictionary<string, ICommand> _commands;

    static async Task Main(string[] args)
    {
        _userInterface = new ConsoleUserInterface();
        _dataManager = new FileManager();

        User user = await CreateNewUserAsync();
        FoodDiaryData foodDiary = new FoodDiaryData();

        // Инициализация команд
        _commands = new Dictionary<string, ICommand>
        {
            { "1", new AddFoodCommand() },
            { "2", new ShowStatisticsCommand() },
            { "3", new ExitCommand(_dataManager, user, foodDiary.Foods) }
        };

        await MainLoopAsync(user, foodDiary);
    }

    private static async Task MainLoopAsync(User user, FoodDiaryData foodDiary)
    {
        bool isRunning = true;
        while (isRunning)
        {
            await _userInterface.WriteMessageAsync("\n1. Добавить продукт\n2. Просмотреть статистику\n3. Выход");
            await _userInterface.WriteMessageAsync("Выберите действие (1-3): ");
            string action = await _userInterface.ReadInputAsync();

            if (_commands.TryGetValue(action, out ICommand command))
            {
                await command.ExecuteAsync("", _userInterface);
                if (action == "3")
                    isRunning = false;
            }
            else
            {
                await _userInterface.WriteMessageAsync("Ошибка! Введите корректный номер действия (1-3).");
            }
        }
    }

    private static async Task<User> CreateNewUserAsync()
    {
        var userInputManager = new UserInputManager(_userInterface);
        User user = new User
        {
            Height = await userInputManager.GetPositiveIntegerAsync("Введите ваш рост (в см): "),
            Weight = await userInputManager.GetPositiveIntegerAsync("Введите ваш вес (в кг): "),
            Age = await userInputManager.GetPositiveIntegerAsync("Введите ваш возраст (в годах): "),
            Gender = await userInputManager.GetGenderAsync(),
            ActivityLevel = await userInputManager.GetActivityLevelAsync(),
            TargetCalories = await userInputManager.GetPositiveIntegerAsync("Введите вашу целевую калорийность (в ккал): ")
        };

        return user;
    }
}
