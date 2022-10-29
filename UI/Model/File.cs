namespace UI.Model;

public class File : IFileSystemObject
{
    public string Name { get; }
    public string Size { get; }
    public double Percentage { get; }

    public File(string name, string size, double percentage)
    {
        Name = name;
        Size = size;
        Percentage = percentage;
    }
}