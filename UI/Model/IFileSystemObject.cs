namespace UI.Model;

public interface IFileSystemObject
{
    string Name { get; }
    long Size { get; }
    double Percentage { get; }
}