namespace Дневник_Питания.Core.Interfaces;

public interface IUserInterface
{
    void WriteMessage(string message);
    string ReadInput();
}