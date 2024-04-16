using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Wordpicker_API.Configs;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.S3Service
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IAppConfigs _config;
        private readonly ApiResponse _response;

        public S3Service(IAppConfigs config) 
        { 
            _config = config;
            _response = new ApiResponse();
            _s3Client = CreateS3Client();
        }
        public async Task<ApiResponse> CreatePreSignedUrl(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> DoesBucketExist(string bucket)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetObjectAsync(string prefix)
        {
            if(string.IsNullOrEmpty(prefix))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "prefix is empty", "");
                return _response;
            }

            var bucket = _config.GetS3BucketName();
            if (!await DoesBucketExist(bucket))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "S3 bucket does not exist", "");
                return _response;
            }

            var parameters = new GetObjectRequest
            {
                BucketName = bucket,
                Key = prefix
            };

            try
            {
                var result = await _s3Client.GetObjectAsync(parameters);
                if(result == null)
                {

                    _response.SetResponse(true, StatusCodes.Status204NoContent, "No item found", "");
                    return _response;
                }

                _response.SetResponse(true, StatusCodes.Status200OK, "Success", JsonConvert.SerializeObject(result));
                return _response;
            } catch (Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }

        public async Task<ApiResponse> PutObjectAsync(string prefix, byte[] data, string contentType)
        {
            throw new NotImplementedException();
        }
        private IAmazonS3 CreateS3Client()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(_config.GetS3AccessKey(), _config.GetS3SecretKey());
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.APNortheast1
            };
            return new AmazonS3Client(awsCredentials, s3Config);
        }
    }
}
