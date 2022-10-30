using System.Collections.ObjectModel;
using Core.Model;
using UI.ViewModel;

namespace UI.Model;

public class ResultTree
{
    public ObservableCollection<IFileSystemObject> Root { get; }

    public ResultTree(Node node)
    {
        Root = new ObservableCollection<IFileSystemObject>{new TreeConverter().ConvertToTree(node)};
    }
}