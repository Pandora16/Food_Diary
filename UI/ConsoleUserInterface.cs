using Дневник_Питания.Core.Interfaces;

namespace Дневник_Питания.UI;

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
