// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Avalonia.Media;

namespace ContactApp.Wpf.Helpers;

public static class AvatarHelper {
  public const string AvatarCacheDirectory = "contactapp-avatar-cache";

  public struct RenderAvatarResult {
    public string? Content { get; init; }
    public IImage? Source { get; set; }
  }

  /// <summary>
  /// Renders the given first name and last name into a avatar prefix
  /// </summary>
  /// <param name="firstName">The user's firstname</param>
  /// <param name="lastName">The user's lastname</param>
  /// <returns>Avatar content</returns>
  public static string RenderContent(string? firstName, string? lastName) {
    return string.Concat((firstName ?? "N")[0], (lastName ?? "A")[0]);
  }

  /// <summary>
  /// Clears the cached avatar of given url
  /// </summary>
  /// <param name="url">The profile picture url</param>
  public static void ClearImage(string url) {
    var path = ImageHelper.GetImageUrlPath(url, AvatarCacheDirectory);

    try {
      File.Delete(path);
    }
    catch {
      // Do nothing
    }
  }

  /// <summary>
  /// Renders the given avatar details and resolves profile url, if failed, returns content only.<br/>
  /// <b>Note:</b> This method download and store then image in temp file and return same image if already downloaded. 
  /// </summary>
  /// <param name="firstName">The user's firstname</param>
  /// <param name="lastName">The user's lastname</param>
  /// <param name="imageUrl">The profile picture url</param>
  /// <returns>The computed results (content / profile image)</returns>
  public static async Task<RenderAvatarResult> RenderSource(string? firstName, string? lastName, string? imageUrl) {
    var content = RenderContent(firstName, lastName);

    var result = new RenderAvatarResult {
      Content = content,
    };

    if (Design.IsDesignMode) {
      return result;
    }

    if (string.IsNullOrEmpty(imageUrl)) {
      return result;
    }

    try {
      var image = await ImageHelper.LoadFromUrlCacheAsync(imageUrl, AvatarCacheDirectory);

      if (image != null) result.Source = image;

      return result;
    }
    catch (Exception e) {
      Console.WriteLine(e);
      return result;
    }
  }
}