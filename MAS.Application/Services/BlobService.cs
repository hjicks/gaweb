using MAS.Application.Interfaces;

namespace MAS.Application.Services;

internal sealed class BlobService : IBlobService
{
    public string? EncodeBlobToBase64(byte[] fileBytes)
    {
        if (fileBytes == null) return null;
        return Convert.ToBase64String(fileBytes);
    }

    public byte[] DecodeBase64Blob(string base64String)
    {
        var cleanBase64String = CleanBase64String(base64String);

        byte[] imageBytes;
        try
        {
            imageBytes = Convert.FromBase64String(cleanBase64String);

            // file sizes more than 50 MB are not allowed
            if (imageBytes.Length > 52_428_800) 
                return Array.Empty<byte>();
        }
        catch (FormatException)
        {
            return Array.Empty<byte>();
        }

        return imageBytes; 
    }

    private static string CleanBase64String(string base64String)
    {
        // handles data URL format like "data:image/jpeg;base64,..."
        if (base64String.Contains("data:"))
        {
            var commaIndex = base64String.IndexOf(','); // returns -1 if not found
            if (commaIndex != -1)
                return base64String[(commaIndex + 1)..];
        }
        return base64String;
    }

    public bool ValidateImageBlob(byte[] imageBytes)
    {
        // image sizes less than 1 KB or more than 2 MB are not allowed
        if (imageBytes.Length < 1024 || imageBytes.Length > 2_097_152)
            return false;

        var signature = BitConverter.ToString(
            imageBytes.Take(12).ToArray()).Replace("-", "").ToLowerInvariant();

        // common image signatures 
        var validSignatures = new[]
        {
            "ffd8ff",      // JPEG 
            "89504e47",    // PNG   
            "47494638",    // GIF 
            "52494646"     // WebP (RIFF header) 
        };

        return validSignatures.Any(sig => signature.StartsWith(sig));
    }
}
