using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ContactApp.Wpf.Helpers;

public static class ImageHelper {
  private static readonly Dictionary<string, string> FileRegistry = [];

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
  /// Loads an image from the given url and saves it into the cache and retrieve later
  /// </summary>
  /// <param name="url">The image url</param>
  /// <returns>Image object</returns>
  public static async Task<Bitmap?> LoadFromUrlCacheAsync(string url) {
    var hash = CryptoHelper.Sha1(url);
    var path = Path.Join(FileHelper.GetTempDir(), hash + ".tmp");

    if (File.Exists(path)) {
      return new Bitmap(File.OpenRead(path));
    }

    _ = await FileHelper.DownloadFileAsync(url, path);
    return new Bitmap(File.OpenRead(path));
  }
}