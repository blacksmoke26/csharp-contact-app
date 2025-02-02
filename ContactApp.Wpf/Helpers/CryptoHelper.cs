using System.Security.Cryptography;
using System.Text;

namespace ContactApp.Wpf.Helpers;

public static class CryptoHelper {
  public static string Sha1(string str) {
    var hashBytes = SHA1.Create().ComputeHash(
      Encoding.UTF8.GetBytes(str)
    );
    return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
  }
}