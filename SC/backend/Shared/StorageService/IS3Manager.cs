namespace backend.Shared.StorageService;

public interface IS3Manager
{
    Task UploadFileAsync(Stream fileStream, string keyName);
    Task<Stream> DownloadFileAsync(string keyName);
    Task DeleteFileAsync(string keyName);
    Task<bool> TestConnectionAsync();
}