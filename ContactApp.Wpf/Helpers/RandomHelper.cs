namespace ContactApp.Wpf.Helpers;

public static class RandomHelper {
  /// <summary>
  /// Get random number from the specified limit
  /// </summary>
  /// <param name="count">Total count</param>
  /// <returns>The random number</returns>
  public static int GetRandomFrom(int count = 100) => new Random().Next(count);
}