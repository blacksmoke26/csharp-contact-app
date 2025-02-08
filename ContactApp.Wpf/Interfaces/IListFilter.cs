// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace ContactApp.Wpf.Interfaces;

public interface IListFilter {
  /// <summary>
  /// Returns all records with current applied filters
  /// </summary>
  /// <returns></returns>
  public Task<object> FetchFiltered();
}