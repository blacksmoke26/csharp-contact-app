using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using ContactApp.Wpf.ViewModels;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ContactApp.Wpf;

public partial class App : Application {
  public override void Initialize() {
    AvaloniaXamlLoader.Load(this);
  }

  public override void OnFrameworkInitializationCompleted() {
    ViewLocator locator = new();
    DataTemplates.Add(locator);

    // configure services
    ConfigureServices();

    // Remove vanilla validation plugins

    #region ValidationPlugins

    // https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins

    // Get an array of plugins to remove
    var dataValidationPluginsToRemove =
      BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

    // remove each entry found
    foreach (var plugin in dataValidationPluginsToRemove) {
      BindingPlugins.DataValidators.Remove(plugin);
    }

    #endregion

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
      // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
      // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
      DisableAvaloniaDataAnnotationValidation();
      desktop.MainWindow = new MainWindow {
        DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>(),
      };
    }

    base.OnFrameworkInitializationCompleted();
  }

  private void DisableAvaloniaDataAnnotationValidation() {
    // Get an array of plugins to remove
    var dataValidationPluginsToRemove =
      BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

    // remove each entry found
    foreach (var plugin in dataValidationPluginsToRemove) {
      BindingPlugins.DataValidators.Remove(plugin);
    }
  }

  /// <summary>
  /// Register DI services<br/>
  /// Reference: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection
  /// </summary>
  private void ConfigureServices() {
    ServiceCollection services = new();

    #region View Models

    services.AddSingleton<MainWindowViewModel>();
    services.AddTransient<ContactFormViewModel>();

    #endregion

    Ioc.Default.ConfigureServices(services.BuildServiceProvider());
  }
}