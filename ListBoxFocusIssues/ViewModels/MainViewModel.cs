using DynamicData;
using DynamicData.Binding;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive;

namespace ListBoxFocusIssues.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    private BabyViewModel _viewModel;
    public BabyViewModel ViewModel
    {
        get => _viewModel;
        set => this.RaiseAndSetIfChanged(ref _viewModel, value);
    }

    public MainViewModel()
    {
        this._viewModel = new BabyViewModel();
        RebuildVM = ReactiveCommand.Create(() =>
        {
            ViewModel = new BabyViewModel();
        });
    }

    public ReactiveCommand<Unit, Unit> RebuildVM { get; } 
}

public class BabyViewModel : ViewModelBase
{
    private ObservableCollection<string> _itemsSource = new();

    private ReadOnlyObservableCollection<string> _itemsDerived;

    public ReadOnlyObservableCollection<string> Items { get => _itemsDerived; }

    private string? _selectedItem;
    public string? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ReactiveCommand<Unit, Unit> RebuildListCommand { get; }

    public BabyViewModel()
    {
        _itemsSource.ToObservableChangeSet()
           .Bind(out _itemsDerived)
           .ObserveOn(RxApp.MainThreadScheduler)
           .Subscribe();

        RebuildList();

        RebuildListCommand = ReactiveCommand.Create(() => RebuildList());
    }

    private void RebuildList()
    {
        _itemsSource.Clear();
        for (int i = 0; i < 20; i++)
        {
            _itemsSource.Add(Random.Shared.Next(1, 100).ToString());
        }
    }
}
