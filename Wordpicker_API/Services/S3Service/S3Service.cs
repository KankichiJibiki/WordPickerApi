using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System.Net;
using Wordpicker_API.Configs;
using Wordpicker_API.Utils;
using static Google.Rpc.Context.AttributeContext.Types;

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
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("Key is null or empty", nameof(key));
            }

            var bucket = _config.GetS3BucketName();
            if (!await DoesBucketExist(bucket))
            {
                throw new ArgumentException($"S3 bucket does not exist {bucket}");
            }

            try
            {
                var objectResponse = await GetObjectAsync(key);

                if (objectResponse.GetResponse().StatusCode.Equals(StatusCodes.Status204NoContent))
                {
                    _response.SetResponse(true, StatusCodes.Status204NoContent, "No object found", "");
                    return _response;
                }

                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucket,
                    Key = key,
                    Expires = DateTime.Now.AddHours(1)
                };

                var preSignedUrl = await _s3Client.GetPreSignedURLAsync(request);

                _response.SetResponse(true, StatusCodes.Status200OK, "Success", preSignedUrl);
                return _response;
            } catch(Exception ex)
            {
                throw new AmazonS3Exception(ex.Message);
            }
        }

        public async Task<Boolean> DoesBucketExist(string bucket)
        {
            return await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucket);
        }

        public async Task<ApiResponse> GetObjectAsync(string prefix)
        {
            if(string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("Prefix is null or empty", nameof(prefix));
            }

            var bucket = _config.GetS3BucketName();
            if (!await DoesBucketExist(bucket))
            {
                throw new ArgumentException($"S3 bucket does not exist {bucket}");
            }

            var parameters = new GetObjectRequest
            {
                BucketName = bucket,
                Key = prefix
            };

            try
            {
                var result = await _s3Client.GetObjectAsync(parameters);
                using (var responseStream = result.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    string content = await reader.ReadToEndAsync();

                    _response.SetResponse(true, StatusCodes.Status200OK, "Success", content);
                    return _response;
                }
            } catch (Exception ex)
            {
                _response.SetResponse(true, StatusCodes.Status204NoContent, ex.Message, "");
                return _response;
            }
        }

        public async Task<ApiResponse> PutObjectAsync(string prefix, byte[] data, string contentType)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("Prefix is null or empty", nameof(prefix));
            }

            var bucket = _config.GetS3BucketName();
            if (!await DoesBucketExist(bucket))
            {
                throw new ArgumentException($"S3 bucket does not exist {bucket}");
            }

            if (!data.Any())
            {
                throw new ArgumentException($"data is empty {data}");
            }

            var key = $"{prefix}.{GetFileExtension(contentType)}";
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = key,
                    InputStream = new MemoryStream(data),
                    ContentType = contentType
                };

                var response = await _s3Client.PutObjectAsync(request);
                _response.SetResponse(true, StatusCodes.Status200OK, "Success", key);
                return _response;
            } catch (Exception ex)
            {
                throw new AmazonS3Exception(ex.Message);
            }
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
        private string GetFileExtension(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return "jpg";
                case "audio/wav":
                    return "wav";
                default:
                    throw new ArgumentException("Unsupported content type", nameof(contentType));
            }
        }
    }
}
