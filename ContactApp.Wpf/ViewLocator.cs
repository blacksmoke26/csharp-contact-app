using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using ContactApp.Wpf.ViewModels;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views;
using ContactApp.Wpf.Views.Forms;

namespace ContactApp.Wpf;

public class ViewLocator : IDataTemplate {
  private readonly Dictionary<Type, Func<Control>> _locator = new();

  public ViewLocator() {
    RegisterViewFactory<MainWindowViewModel, MainWindow>();
    RegisterViewFactory<ContactFormViewModel, ContactFormView>();
  }

  public Control? Build(object? data) {
    if (data is null || !_locator.TryGetValue(data.GetType(), out var factory))
      return new TextBlock { Text = "No ViewModel provided" };

    return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {data.GetType()}" };
  }

  public bool Match(object? data) {
    return data is ViewModelBase or ObservableObject;
  }

  private void RegisterViewFactory<TViewModel, TView>()
    where TViewModel : class
    where TView : Control {
    _locator.Add(typeof(TViewModel), Activator.CreateInstance<TView>);
  }
}