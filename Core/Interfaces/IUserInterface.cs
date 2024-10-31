namespace Дневник_Питания.Core.Interfaces;

public interface IUserInterface
{
    Task<string> ReadInputAsync();
    Task WriteMessageAsync(string message);
}
