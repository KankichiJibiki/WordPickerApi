using Google.Cloud.TextToSpeech.V1;
using Wordpicker_API.Configs;
using Wordpicker_API.DTOs;
using Wordpicker_API.Services.S3Service;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.TextToSpeechService
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private static readonly string MALE_VOICE_CODE = "m";
        private static readonly string FEMALE_VOICE_CODE = "f";
        private readonly IAppConfigs _config;
        private readonly ApiResponse _response;
        private readonly IS3Service _s3Service;
        private readonly TextToSpeechClient _textClient;

        public TextToSpeechService(IAppConfigs config, IS3Service s3Service) 
        {
            _config = config;
            _response = new ApiResponse();
            _s3Service = s3Service;
            _textClient = TextToSpeechClient.Create();
        }
        public async Task<ApiResponse> ConvertTextToSpeech(TextToSpeechRequestDto request)
        {
            if(string.IsNullOrEmpty(request.Text))
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Text was either null or empty", "");
                return _response;
            }

            if(request.AudioGender != MALE_VOICE_CODE && request.AudioGender != FEMALE_VOICE_CODE)
            {
                _response.SetResponse(false, StatusCodes.Status400BadRequest, "Invalid request on audio gender", request.AudioGender);
                return _response;
            }

            try
            {
                SynthesizeSpeechRequest synthesizeRequest = new SynthesizeSpeechRequest()
                {
                    Input = new SynthesisInput
                    {
                        Text = request.Text
                    },
                    Voice = new VoiceSelectionParams 
                    {
                        LanguageCode = request.LanguageCode,
                        SsmlGender = request.AudioGender == MALE_VOICE_CODE ? SsmlVoiceGender.Male : SsmlVoiceGender.Female,
                        Name = "en-US-Wavenet-D"
                    },
                    AudioConfig = new AudioConfig
                    {
                        AudioEncoding = AudioEncoding.Mp3
                    }
                };

                SynthesizeSpeechResponse synthesizeResponse = _textClient.SynthesizeSpeech(synthesizeRequest);
                byte[] audioData = synthesizeResponse.AudioContent.ToByteArray();

                var putObjectResponse = await _s3Service.PutObjectAsync($"{_config.GetTempAudioPrefix()}/{request.Text}", audioData, _config.GetAudioContentType());
                if (!putObjectResponse.GetResponse().Success)
                {
                    throw new FileLoadException("Failed to put audio file");
                }

                return await _s3Service.CreatePreSignedUrl(putObjectResponse.GetResponse().Data);
            } catch(Exception ex)
            {
                _response.SetResponse(false, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }


    }
}
