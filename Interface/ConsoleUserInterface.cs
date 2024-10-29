namespace Дневник_Питания.Interface;

public class ConsoleUserInterface : IUserInterface
{
    public async Task WriteMessageAsync(string message)
    {
        await Task.Run(() => Console.WriteLine(message));
    }

    public async Task<string> ReadInputAsync()
    {
        return await Task.Run(() => Console.ReadLine() ?? string.Empty);
    }
}