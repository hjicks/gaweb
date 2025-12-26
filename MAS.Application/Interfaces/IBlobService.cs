namespace MAS.Application.Interfaces;

public interface IBlobService
{
    public string EncodeBlobToBase64(byte[] fileBytes);
    byte[] DecodeBase64Blob(string base64String);
    bool ValidateImageBlob(byte[] imageBytes);
}
