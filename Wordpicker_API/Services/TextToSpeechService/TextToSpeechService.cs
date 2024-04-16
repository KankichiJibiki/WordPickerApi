using Google.Cloud.TextToSpeech.V1;
using Wordpicker_API.DTOs;
using Wordpicker_API.Utils;

namespace Wordpicker_API.Services.TextToSpeechService
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private static readonly string MALE_VOICE_CODE = "m";
        private static readonly string FEMALE_VOICE_CODE = "f";
        private readonly IConfiguration _config;
        private readonly ApiResponse _response;
        private readonly TextToSpeechClient _textClient;

        public TextToSpeechService(IConfiguration config) 
        {
            _config = config;
            _response = new ApiResponse();
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

            } catch(Exception ex)
            {
                _response.SetResponse(false, StatusCodes.Status500InternalServerError, ex.Message, "");
                return _response;
            }
        }
    }
}
