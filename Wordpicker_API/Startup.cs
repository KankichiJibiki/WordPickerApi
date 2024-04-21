using Wordpicker_API.Configs;
using Wordpicker_API.Services.DeepLService;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Services.S3Service;
using Wordpicker_API.Services.TextToSpeechService;
using Wordpicker_API.Services.WordsApiService;

namespace Wordpicker_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Configure services to DI(Dependency Injection Container)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();

            services.AddScoped<IAppConfigs, AppConfigs>();
            services.AddScoped<IWordsApiService, WordsApiService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IDeepLService, DeepLService>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<ITextToSpeechService, TextToSpeechService>();
        }

        // Configure HTTP pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Environment setting
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // Cors settings
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            // Set Routing
            app.UseRouting();

            // Set and map controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
