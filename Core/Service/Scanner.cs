using System.Collections.Concurrent;
using Core.exception;
using Core.Model;

namespace Core.Service;

public class Scanner : IScanner
{
    private ConcurrentQueue<Node> _queue = new ConcurrentQueue<Node>();
    private CancellationTokenSource _tokenSource = new CancellationTokenSource();
    private SemaphoreSlim _semaphore;

    public Node StartScan(string path, int threadCount)
    {
        if (threadCount <= 0)
        {
            throw new IllegalThreadCountException("Thread count <= 0");
        }
        
        if (File.Exists(path))
        {
            FileInfo file = new FileInfo(path);
            return new Node(file.Name, file.FullName, file.Length, null);
        }

        if (!Directory.Exists(path))
        {
            throw new InvalidPathException("Path: " + path);
        }

        _semaphore = new SemaphoreSlim(threadCount);
        CancellationToken token = _tokenSource.Token;
        
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        Node root = new Node(directoryInfo.Name, directoryInfo.FullName, null);
        
        Scan(root, token);
        do
        {
            if (_queue.TryDequeue(out var node))
            {
                try
                {
                    _semaphore.Wait(token);
                    Task.Run(() =>
                    {
                        Scan(node, token);
                        _semaphore.Release();
                    }, token);
                }
                catch (Exception)
                {
                    
                }
            }
        } while (!token.IsCancellationRequested &&
                 (_semaphore.CurrentCount != threadCount || !_queue.IsEmpty));
        
        return root;
    }

    public void CancelScan()
    {
        _tokenSource.Cancel();
    }

    private void Scan(Node node, CancellationToken token)
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(node.FullPath);
        
        DirectoryInfo[] childrenDirectories;
        try
        {
            childrenDirectories = currentDirectory.GetDirectories();
        }
        catch (Exception e)
        {
            return;
        }
            
        foreach (var directory in childrenDirectories)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            Node childNode = new Node(directory.Name, directory.FullName, node);
            node.ChildrenNodes.Add(childNode);
            _queue.Enqueue(childNode);
        }

        FileInfo[] files = currentDirectory.GetFiles();
        long dirLength = 0;
        foreach (var file in files)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            long fileLength = 0;
            if (file.Extension != ".lnk")
            {
                fileLength = file.Length;
            }
            
            node.ChildrenNodes.Add(new Node(file.Name, file.FullName, fileLength, node));
            dirLength += fileLength;
        }

        node.Size = dirLength;
        
        while (node.Parent != null)
        {
            node.Parent.Size += dirLength;
            node = node.Parent;
        }
    }
}