﻿using System.Collections.Concurrent;
using Core.exception;
using Core.Model;

namespace Core.Service;

public class Scanner : IScanner
{
    private ConcurrentQueue<Node> _queue = new ConcurrentQueue<Node>();
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
            return new Node(file.Name, file.FullName, file.Length);
        }

        if (!Directory.Exists(path))
        {
            throw new InvalidPathException("Path: " + path);
        }

        _semaphore = new SemaphoreSlim(threadCount);
        
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        Node root = new Node(directoryInfo.Name, directoryInfo.FullName);
        
        Scan(root);
        do
        {
            if (_queue.TryDequeue(out var node))
            {
                _semaphore.Wait();
                Task.Run(() =>
                {
                    Scan(node);
                    _semaphore.Release();
                }); 
            }
        } while (_semaphore.CurrentCount != threadCount || !_queue.IsEmpty);
        
        return root;
    }

    private void Scan(Node node)
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(node.FullPath);
        
        DirectoryInfo[] childrenDirectories = currentDirectory.GetDirectories();
        foreach (var directory in childrenDirectories)
        {
            Node childNode = new Node(directory.Name, directory.FullName);
            node.ChildrenNodes.Add(childNode);
            _queue.Enqueue(childNode);
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
            //node.Size += fileLength;
        }
    }

    public void StopScan()
    {
        throw new NotImplementedException();
    }
}