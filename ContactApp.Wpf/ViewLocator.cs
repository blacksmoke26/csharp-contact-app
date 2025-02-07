using Avalonia.Controls.Templates;
using ContactApp.Wpf.ViewModels;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views;
using ContactApp.Wpf.Views.Forms;

namespace ContactApp.Wpf;

public class ViewLocator : IDataTemplate {
  /// <summary>
  /// Lookup ViewModel~View Dictionary for view instances 
  /// </summary>
  private readonly Dictionary<Type, Func<Control>> _locator = new();

  public ViewLocator() {
    RegisterViewFactory<MainWindowViewModel, MainWindow>();
    RegisterViewFactory<ContactFormViewModel, ContactFormView>();
  }

  /// <summary>
  /// Creates a view instance against the view model 
  /// </summary>
  /// <param name="data">View Model instance</param>
  /// <returns>View instance</returns>
  public Control Build(object? data) {
    if (data is null || !_locator.TryGetValue(data.GetType(), out var factory))
      return new TextBlock { Text = "ViewModel Not Found:" };

    return factory.Invoke();
  }

  /// <summary>
  /// Checks whatever the given object is a ViewModel instance
  /// </summary>
  /// <param name="data">The object</param>
  /// <returns>True when matched, false otherwise</returns>
  public bool Match(object? data) {
    return data is ViewModelBase or ObservableObject;
  }

  /// <summary>
  /// Adds the ViewModel / View into the <see cref="_locator"/> dictionary
  /// </summary>
  private void RegisterViewFactory<TViewModel, TView>()
    where TViewModel : class
    where TView : Control {
    _locator.Add(typeof(TViewModel), Activator.CreateInstance<TView>);
  }

  /// <summary>
  /// Creates view instances based on the view-model
  /// </summary>
  /// <param name="fn">A callback function to manipulate created instance</param>
  /// <typeparam name="TViewModel">ViewModel class</typeparam>
  /// <returns>The created view instance</returns>
  public Control CreateView<TViewModel>(Action<TViewModel>? fn = null)
    where TViewModel : class {
    var viewModel = Ioc.Default.GetRequiredService<TViewModel>();
    fn?.Invoke(viewModel);
    var control = Build(viewModel);
    control.DataContext = viewModel;
    return control;
  }

  /// <summary>
  /// Creates view instances based on the given view-model instance
  /// </summary>
  /// <param name="vm">The viewmodel instance</param>
  /// <typeparam name="TViewModel">ViewModel class</typeparam>
  /// <returns>The created view instance</returns>
  public Control CreateView<TViewModel>(TViewModel vm)
    where TViewModel : class {
    var control = Build(vm);
    control.DataContext = vm;
    return control;
  }
}