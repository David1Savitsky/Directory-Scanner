using System.Collections.ObjectModel;

namespace UI.Model;

public class Directory : IFileSystemObject
{
    public string Name { get; }
    public long Size { get; }
    public double Percentage { get; }
    public ObservableCollection<IFileSystemObject> ChildrenNodes { get; }

    public Directory(string name, long size, double percentage)
    {
        Name = name;
        Size = size;
        Percentage = percentage;
        ChildrenNodes = new ObservableCollection<IFileSystemObject>();
    }
}