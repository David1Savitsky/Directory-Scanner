namespace Core.Model;

public class FileSystemTree
{
    private Node Root { get; }
    
    public FileSystemTree(Node root)
    {
        Root = root;
    }
}