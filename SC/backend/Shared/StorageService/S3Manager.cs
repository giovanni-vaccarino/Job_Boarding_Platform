using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace backend.Shared.StorageService;


public class S3Manager : IS3Manager
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

    public S3Manager(IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");
        var accessKey = awsOptions["AccessKey"];
        var secretKey = awsOptions["SecretKey"];
        var region = awsOptions["Region"];
        var bucketName = awsOptions["BucketName"];
        
        if (accessKey == null || secretKey == null || region == null || bucketName == null)
        {
            throw new Exception("AWS credentials not found in appsettings.json");
        }

        _bucketName = bucketName;
        _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
    }
    
    public async Task UploadFileAsync(Stream fileStream, string keyName)
    {
        try
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(fileStream, _bucketName, keyName);
            Console.WriteLine($"File uploaded: {keyName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string keyName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName
            };

            using (var response = await _s3Client.GetObjectAsync(request))
            {
                MemoryStream memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteFileAsync(string keyName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName
            };

            await _s3Client.DeleteObjectAsync(request);
            Console.WriteLine($"File deleted: {keyName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
            throw;
        }
    }
    
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                MaxKeys = 1 
            };

            await _s3Client.ListObjectsV2Async(request);

            Console.WriteLine("S3 connection successful.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"S3 connection test failed: {ex.Message}");
            return false;
        }
    }
}
