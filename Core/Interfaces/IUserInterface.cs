namespace Дневник_Питания.Core.Interfaces;

public interface IUserInterface
{
    Task WriteMessageAsync(string message);
    Task<string> ReadInputAsync();
}
