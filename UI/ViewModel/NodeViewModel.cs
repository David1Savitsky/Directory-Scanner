using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core.Service;
using Microsoft.Win32;
using UI.Model;

namespace UI.ViewModel;

public class NodeViewModel : INotifyPropertyChanged
{

    private const int ThreadCount = 10;
    private IScanner _scanner = new Scanner();
    
    private string _scanningPath;
    public string ScanningPath
    {
        get => _scanningPath;
        set
        {
            _scanningPath = value;
            OnPropertyChanged(nameof(ScanningPath));
        }
    }

    private ResultTree _root;
    public ResultTree Root
    {
        get => _root;
        private set
        {
            _root = value;
            OnPropertyChanged(nameof(Root));
        }
    }
    
    public ICommand ScanCommand => new OpenFileCommand(obj => Scan());
    private void Scan()
    {

        var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
        if (dialog.ShowDialog().GetValueOrDefault())
        {
            ScanningPath = dialog.SelectedPath;
        }
        
        Task.Run(() =>
        {
            var result = _scanner.StartScan(ScanningPath, ThreadCount);
            Root = new ResultTree(result);
        });

    }

    public ICommand CancelCommand => new OpenFileCommand(_ => Cancel());

    private void Cancel()
    {
        
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}