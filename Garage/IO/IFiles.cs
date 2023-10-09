namespace Garage.IO
{
    public interface IFiles
    {
        bool Exists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }
}