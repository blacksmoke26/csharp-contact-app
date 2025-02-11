// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Ursa.Controls;

namespace ContactApp.Wpf.Views;

public partial class MainWindow : UrsaWindow {
  public MainWindow() {
    InitializeComponent();
    
    // attached events
    Resized += MainWindow_Resized;
  }

  /// <summary>
  /// Event triggers when window is being resized
  /// </summary>
  /// <param name="sender">The sender object</param>
  /// <param name="e">Event arguments</param>
  private void MainWindow_Resized(object? sender, WindowResizedEventArgs e) {
    var elm = this.FindControl<ScrollViewer>("ListingScrollViewer");

    if (elm != null) {
      elm.Height = e.ClientSize.Height - 259; // - Search input control height
    }
  }

  /// <summary>
  /// Auto focus search query textbox on loaded
  /// </summary>
  /// <param name="sender">The sender object</param>
  /// <param name="e">Event arguments</param>
  private void SearchTextBox_OnLoaded(object? sender, RoutedEventArgs e) {
    (sender as TextBox)?.Focus();
  }
}