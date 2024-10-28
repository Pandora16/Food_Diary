using Дневник_Питания.Consol;

namespace Дневник_Питания.ComandManager

{
    public interface ICommand
    {
        Task ExecuteAsync(string message, IUserInterface userInterface);
    }
}