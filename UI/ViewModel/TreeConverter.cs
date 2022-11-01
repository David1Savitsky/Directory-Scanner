using System;
using Core.Model;
using UI.Model;

namespace UI.ViewModel;

public class TreeConverter
{

    public IFileSystemObject ConvertToTree(Node root)
    {
        IFileSystemObject fileSystemRoot;
        if (root.ChildrenNodes.Count == 0)
        {
            fileSystemRoot = new File(root.Name, root.Size, 100);
        }
        else
        {
            fileSystemRoot = new Directory(root.Name, root.Size, 100);
            foreach (var child in root.ChildrenNodes)
            {
                ((Directory)fileSystemRoot).ChildrenNodes.Add(Convert(child, root.Size));
            }
        }

        return fileSystemRoot;
    }

    private IFileSystemObject Convert(Node node, long parentNodeSize)
    {
        IFileSystemObject fileSystemObject;
        double percentage = (double)node.Size / parentNodeSize * 100;
        if (node.ChildrenNodes.Count == 0)
        {
            fileSystemObject = new File(node.Name, node.Size, percentage);
        }
        else
        {
            fileSystemObject = new Directory(node.Name, node.Size, percentage);
            foreach (var child in node.ChildrenNodes)
            {
                ((Directory)fileSystemObject).ChildrenNodes.Add(Convert(child, node.Size));
            }
        }

        return fileSystemObject;
    }
}