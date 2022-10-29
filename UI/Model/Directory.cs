using System.Collections.ObjectModel;

namespace UI.Model;

public class Directory : IFileSystemObject
{
    public string Name { get; }
    public string Size { get; }
    public double Percentage { get; }
    public ObservableCollection<IFileSystemObject> ChildrenNodes { get; }

    public Directory(string name, string size, double percentage)
    {
        Name = name;
        Size = size;
        Percentage = percentage;
    }
}