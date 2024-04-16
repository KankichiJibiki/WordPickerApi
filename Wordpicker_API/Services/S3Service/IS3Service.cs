using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.S3Service
{
    public interface IS3Service
    {
        Task<ApiResponse> GetObjectAsync(string prefix);
        Task<ApiResponse> PutObjectAsync(string prefix, byte[] data, string contentType);
        Task<ApiResponse> CreatePreSignedUrl(string key);
        Task<Boolean> DoesBucketExist(string bucket);
    }
}
