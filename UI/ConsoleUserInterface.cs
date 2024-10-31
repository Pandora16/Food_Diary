using Дневник_Питания.Core.Interfaces;

public class ConsoleUserInterface : IUserInterface
{
    public void WriteMessage(string message)
    {
        Console.WriteLine(message);
    }

    public string ReadInput()
    {
        return Console.ReadLine();
    }
}