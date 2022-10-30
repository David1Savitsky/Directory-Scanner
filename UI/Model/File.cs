namespace UI.Model;

public class File : IFileSystemObject
{
    public string Name { get; }
    public long Size { get; }
    public double Percentage { get; }

    public File(string name, long size, double percentage)
    {
        Name = name;
        Size = size;
        Percentage = percentage;
    }
}