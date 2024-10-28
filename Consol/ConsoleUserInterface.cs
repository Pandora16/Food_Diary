using Дневник_Питания.Consol;

public class ConsoleUserInterface : IUserInterface
{
    public async Task WriteMessageAsync(string message)
    {
        Console.WriteLine(message);
    }

    public async Task<string> ReadInputAsync()
    {
        return Console.ReadLine();
    }
}