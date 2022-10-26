using System.Collections.ObjectModel;

namespace Core.Model;

public class Node
{
    public string Name { get; }
    public string FullPath { get; }
    public long Size { get; set; }
    public ObservableCollection<Node> ChildrenNodes { get; }

    public Node(string name, string fullPath, long size)
    {
        Name = name;
        FullPath = fullPath;
        Size = size;
        ChildrenNodes = new ObservableCollection<Node>();
    }

    public Node(string name, string fullPath)
    {
        Name = name;
        FullPath = fullPath;
        ChildrenNodes = new ObservableCollection<Node>();
    }
}