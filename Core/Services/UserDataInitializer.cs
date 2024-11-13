using System;
using System.IO;
using System.Threading.Tasks;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Models;
using Microsoft.Extensions.Logging;

namespace Дневник_Питания.Core.Services
{
    public class UserDataInitializer : IUserDataInitializer
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IUserCreator _userCreator;
        private readonly ILogger<UserDataInitializer> _logger;
        private readonly string _filePath;

        public UserDataInitializer(IFoodRepository foodRepository, IUserCreator userCreator, ILogger<UserDataInitializer> logger)
        {
            _foodRepository = foodRepository;
            _userCreator = userCreator;
            _logger = logger;
            _filePath = Path.GetFullPath("foodDiary.json");
        }

        public async Task<User> InitializeUserDataAsync()
        {
            _logger.LogInformation($"Инициализация данных. Проверка файла: {_filePath}");

            if (File.Exists(_filePath))
            {
                _logger.LogInformation("Файл найден. Предложение очистить или сохранить данные.");
                Console.WriteLine("Хотите очистить данные и начать заново? (да/нет)");
                string response = Console.ReadLine()?.ToLower();
                
                if (response == "да")
                {
                    File.Delete(_filePath);
                    _logger.LogInformation("Данные очищены.");
                    return await _userCreator.CreateNewUserAsync();
                }
                else if (response == "нет")
                {
                    try
                    {
                        _logger.LogInformation("Загрузка данных из файла...");
                        var (user, _) = await _foodRepository.LoadDataAsync(); 
                        if (user == null)
                        {
                            _logger.LogError("Ошибка: данные пользователя не были загружены.");
                            Environment.Exit(1);
                        }
                        _logger.LogInformation("Данные пользователя успешно загружены.");
                        return user;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ошибка при чтении файла {_filePath}: {ex.Message}");
                        throw;
                    }
                }
                else
                {
                    _logger.LogWarning("Неверный ввод. Повторный запрос.");
                    return await InitializeUserDataAsync();
                }
            }
            else
            {
                _logger.LogWarning("Файл данных не найден. Инициализация нового пользователя.");
                return await _userCreator.CreateNewUserAsync();
            }
        }
    }
}
