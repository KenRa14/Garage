namespace Garage.UserInterface
{
    public interface IUI
    {
        void Clear();
        string GetInput();
        void Print<T>(T message);
        void PrintError(string errorMessage);
        void PrintLine(string line);
        void PrintTitle(string title);
    }
}