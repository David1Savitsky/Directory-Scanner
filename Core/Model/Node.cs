using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Core.Model;

public class Node
{
    public string Name { get; }
    public string FullPath { get; }
    public long Size { get; set; }
    [JsonIgnore]
    public Node Parent { get; }
    public ObservableCollection<Node> ChildrenNodes { get; }

    public Node(string name, string fullPath, long size, Node parent)
    {
        Name = name;
        FullPath = fullPath;
        Size = size;
        Parent = parent;
        ChildrenNodes = new ObservableCollection<Node>();
    }

    public Node(string name, string fullPath, Node parent)
    {
        Name = name;
        FullPath = fullPath;
        Parent = parent;
        ChildrenNodes = new ObservableCollection<Node>();
    }
}