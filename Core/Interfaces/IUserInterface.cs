namespace Дневник_Питания.Core.Interfaces;

// Интерфейс для пользовательского интерфейса
public interface IUserInterface
{
    Task WriteMessageAsync(string message);
    Task<string> ReadInputAsync();
}