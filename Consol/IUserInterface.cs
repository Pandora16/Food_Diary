namespace Дневник_Питания.Consol;

// Интерфейс для пользовательского интерфейса
public interface IUserInterface
{
    Task WriteMessageAsync(string message);
    Task<string> ReadInputAsync();
}

public class ConsoleUserInterface : IUserInterface
{
    public async Task WriteMessageAsync(string message)
    {
        Console.WriteLine(message);
        await Task.CompletedTask; // Заменяем заглушкой, чтобы метод был асинхронным
    }

    public async Task<string> ReadInputAsync()
    {
        return await Task.FromResult(Console.ReadLine());
    }
}