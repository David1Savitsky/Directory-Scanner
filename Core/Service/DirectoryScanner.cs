using Core.exception;
using Core.Model;

namespace Core.Service;

public class DirectoryScanner : IDirectoryScanner
{
    public FileSystemTree StartScan(string path, int threadCount)
    {
        if (threadCount <= 0)
        {
            throw new IllegalThreadCountException("Thread count <= 0");
        }

        //TODO Check
        if (File.Exists(path))
        {
            FileInfo file = new FileInfo(path);
            return new FileSystemTree(new Node(file.Name, file.FullName, file.Length));
        }

        if (!Directory.Exists(path))
        {
            throw new InvalidPathException("Path: " + path);
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        Node root = new Node(directoryInfo.Name, directoryInfo.FullName);
        
        Scan(root);
        
    }

    private void Scan(Node node)
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(node.FullPath);
        
        DirectoryInfo[] childrenDirectories = currentDirectory.GetDirectories();
        foreach (var directory in childrenDirectories)
        {
            node.ChildrenNodes.Add(new Node(directory.Name, directory.FullName));
        }

        FileInfo[] files = currentDirectory.GetFiles();
        foreach (var file in files)
        {
            if (file.LinkTarget != null)
            {
                continue;
            }

            long fileLength = file.Length;
            node.ChildrenNodes.Add(new Node(file.Name, file.FullName, fileLength));
            node.Size += fileLength;
        }
    }

    public void StopScan()
    {
        throw new NotImplementedException();
    }
}