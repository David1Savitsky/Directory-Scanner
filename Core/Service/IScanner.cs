using Core.Model;

namespace Core.Service;

public interface IScanner
{
    Node StartScan(string path, int threadCount);
    void CancelScan();
}