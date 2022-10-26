using Core.Model;

namespace Core.Service;

public interface IDirectoryScanner
{
    FileSystemTree StartScan(string path, int threadCount);
    void StopScan();
}