namespace Дневник_Питания.Interface;

public interface IUserInterface
{
    Task WriteMessageAsync(string message);
    Task<string> ReadInputAsync();
}