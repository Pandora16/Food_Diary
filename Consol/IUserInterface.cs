namespace Дневник_Питания.Consol;

// Интерфейс для пользовательского интерфейса
public interface IUserInterface
{
    Task WriteMessageAsync(string message);
    Task<string> ReadInputAsync();
}