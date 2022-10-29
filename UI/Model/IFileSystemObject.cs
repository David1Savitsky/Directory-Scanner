namespace UI.Model;

public interface IFileSystemObject
{
    string Name { get; }
    string Size { get; }
    double Percentage { get; }
}