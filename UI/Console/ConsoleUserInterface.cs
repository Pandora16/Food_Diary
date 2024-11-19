using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Дневник_Питания.Core.Interfaces;

public class ConsoleUserInterface : IUserInterface
{
    public async Task<string> ReadInputAsync()
    {
        return await Task.Run(() => Console.ReadLine());
    }

    public async Task WriteMessageAsync(string message)
    {
        await Task.Run(() => Console.WriteLine(message));
    }
}