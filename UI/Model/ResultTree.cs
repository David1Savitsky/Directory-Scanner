using System.Collections.ObjectModel;

namespace UI.Model;

public class ResultTree
{
    public ObservableCollection<IFileSystemObject> Root { get; }

    public ResultTree()
    {
        Root = new ObservableCollection<IFileSystemObject>();
    }
}