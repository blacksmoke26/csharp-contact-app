using System;
using Avalonia;
using Avalonia.Controls;
using ContactApp.Wpf.ViewModels;
using Dumpify;

namespace ContactApp.Wpf.Controls;

public partial class Sidebar : UserControl {
  public static readonly StyledProperty<string> SelectedProperty = AvaloniaProperty.Register<Sidebar, string>(
    nameof(Selected));

  public string Selected {
    get => GetValue(SelectedProperty);
    set => SetValue(SelectedProperty, value);
  }

  public Sidebar() {
    InitializeComponent();
    Selected.Dump();
    
    DataContext = new SidebarViewModel {
      Selected = Selected
    };
  }
}