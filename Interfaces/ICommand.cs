using Дневник_Питания.Interface;

namespace Дневник_Питания.ComandManager

{
    public interface ICommand
    {
        Task ExecuteAsync(string message, IUserInterface userInterface);
    }
}