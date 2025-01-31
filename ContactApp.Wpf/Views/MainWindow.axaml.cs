// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ContactApp.Wpf.Controls;

namespace ContactApp.Wpf.Views;

public partial class MainWindow : Window {
  public MainWindow() {
    InitializeComponent();
  }

  private void SidebarControl_OnSelectedChange(object? sender, RoutedEventArgs e) {
    var source = (SidebarControl)e.Source!;
    Console.WriteLine($"Selected Value changed: {source.Selected}");
  }
}