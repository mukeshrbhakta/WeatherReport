using System.Security.Cryptography;
using System.Text;

namespace JbHifi.WeatherReport.Common;

/// <summary>
/// Security related stuff 
/// </summary>
public static class Security
{
     /// <summary>
    /// Base64 Initial vector (can be moved to key vault)
    /// </summary>
    private static string InitialVector => "7Hg/nQK4fc9gEp197F780w==";

    /// <summary>
    /// Create a one time AES combination
    /// </summary>
    public static AesContainer CreateAes()
    {
        using var aes = Aes.Create(); 
        return new AesContainer
        {
            Key = Convert.ToBase64String(aes.Key),
            Iv = Convert.ToBase64String(aes.IV)
        };
    }

    /// <summary>
    /// Decrypt the string
    /// </summary>
    /// <param name="secretKey">the b64 encoded secret key</param>
    /// <param name="input">the base 64 input</param>
    /// <returns>clear text decrypted string</returns>
    public static string Decrypt(string? secretKey, string? input)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("secretKey cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("input cannot be null or empty");
        }

        // Create Aes 
        using var aes = Aes.Create();

        // Normalize the contents
        var key = Convert.FromBase64String(secretKey);
        var iv = Convert.FromBase64String(InitialVector);

        // Create a decrypter
        var decryptor = aes.CreateDecryptor(key, iv);
        // Create the streams used for decryption.
        var cipherText = Convert.FromBase64String(input);
        using var ms = new MemoryStream(cipherText);
        // Create crypto stream
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        // Read crypto stream
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Method used by Candidate status APIs
    /// </summary>
    /// <param name="secretKey">the secret key</param>
    /// <param name="iv">the IV</param>
    /// <param name="input">the enc buffer</param>
    /// <returns>clear text response</returns>
    public static string Decrypt(string secretKey, string iv, string? input)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("secretKey cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(iv))
        {
            throw new ArgumentException("iv cannot be null or empty");
        }
        
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("input cannot be null or empty");
        }
        
        // Create Aes 
        using var aes = Aes.Create();
        var keyBytes = Convert.FromBase64String(secretKey);
        var ivBytes = Convert.FromBase64String(InitialVector);
        
        var decryptor = aes.CreateDecryptor(keyBytes, ivBytes);
        // Create the streams used for decryption.
        var cipherText = Convert.FromBase64String(input);
        using var ms = new MemoryStream(cipherText);
        // Create crypto stream
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        // Read crypto stream
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Encrypt the string
    /// </summary>
    /// <param name="secretKey">the b64 encoded secret key</param>
    /// <param name="input">the clear text input</param>
    /// <returns>a base64 encrypted string</returns>
    public static string Encrypt(string? secretKey, string input)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("secretKey cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("input cannot be null or empty");
        }

        // Normalize the contents
        var key = Convert.FromBase64String(secretKey);
        var iv = Convert.FromBase64String(InitialVector);
        var inputBuffer = Encoding.UTF8.GetBytes(input);

        // Create Aes 
        using var aes = Aes.Create(); 

        // Create encryptor
        var encryptor = aes.CreateEncryptor(key, iv);
        // Create MemoryStream
        using var memoryStream = new MemoryStream();
        // Create crypto stream
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        // Write data to a stream 
        cryptoStream.Write(inputBuffer, 0, inputBuffer.Length);
        cryptoStream.FlushFinalBlock();
         
        var encrypted = memoryStream.ToArray();
        // Return encoded encrypted data
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Get hash value
    /// </summary>
    /// <param name="source">the source</param>
    /// <returns></returns>
    public static string GetHash(string source)
    {
        var data = Encoding.Default.GetBytes(source);
        using var sha = SHA256.Create();
        var response = sha.ComputeHash(data);
        return Convert.ToBase64String(response);
    }
}