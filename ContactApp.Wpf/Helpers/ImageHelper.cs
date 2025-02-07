// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ContactApp.Wpf.Helpers;

public static class ImageHelper {
  /// <summary>
  /// Random temp directory name to cache downloaded files
  /// </summary>
  public static readonly string RandomDirectory = Directory.CreateTempSubdirectory().Name;

  /// <summary>
  /// Loads the specified image from the application resources
  /// </summary>
  /// <param name="resourcePath">The resource path to image</param>
  /// <returns>The bitmap object</returns>
  public static Bitmap LoadFromResource(string resourcePath) {
    Uri resourceUri;

    if (!resourcePath.StartsWith("avares://")) {
      var assemblyName = Assembly.GetCallingAssembly().GetName().Name;
      resourceUri = new($"avares://{assemblyName}/{resourcePath.TrimStart('/')}");
    }
    else
      resourceUri = new Uri(resourcePath);

    return new(AssetLoader.Open(resourceUri));
  }

  /// <summary>
  /// Loads an image from the given url
  /// </summary>
  /// <param name="url">The image url</param>
  /// <returns>Image object</returns>
  public static async Task<Bitmap?> LoadFromUrlAsync(string url) {
    if (!url.StartsWith("http") || !Uri.TryCreate(url, UriKind.Absolute, out var uri)) {
      return null;
    }

    using var client = new HttpClient();

    try {
      var data = await client.GetByteArrayAsync(uri);
      return new(new MemoryStream(data));
    }
    catch (HttpRequestException e) {
      Console.WriteLine($"An error occurred while downloading image from '{uri}': {e.Message}");
      return null;
    }
  }

  /// <summary>
  /// Returns a url hashed temporary file path 
  /// </summary>
  /// <param name="url">The image url</param>
  /// <param name="tempDir">(optional) temp directory name</param>
  /// <returns>Absolute temp image path</returns>
  public static string GetImageUrlPath(string url, string? tempDir = null) {
    return FileHelper.GetEnsuredTempDir(tempDir ?? RandomDirectory, $"{CryptoHelper.Sha1(url)}.tmp");
  }

  /// <summary>
  /// Loads an image from the given url and saves it into the cache and retrieve later
  /// </summary>
  /// <param name="url">The image url</param>
  /// <param name="tempDir">(optional) temp directory name</param>
  /// <returns>The image object</returns>
  public static async Task<Bitmap?> LoadFromUrlCacheAsync(string url, string? tempDir = null) {
    var path = GetImageUrlPath(url, tempDir);

    if (File.Exists(path)) {
      return new Bitmap(File.OpenRead(path));
    }

    var result = await FileHelper.DownloadFileAsync(url, path);
    if (result == null) {
      return null;
    }

    await using var stream = File.OpenRead(result);
    return new Bitmap(stream);
  }
}