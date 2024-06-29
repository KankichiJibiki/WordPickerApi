using Google.Api;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Wordpicker_API.Configs;
using Wordpicker_API.Contexts;
using Wordpicker_API.Services.DeepLService;
using Wordpicker_API.Services.HttpService;
using Wordpicker_API.Services.RedisService;
using Wordpicker_API.Services.S3Service;
using Wordpicker_API.Services.TextToSpeechService;
using Wordpicker_API.Services.WordsApiService;
using Wordpicker_API.ValueDefinitions;

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
            services.AddHealthChecks();

            services.AddScoped<IAppConfigs, AppConfigs>();
            services.AddScoped<IWordsApiService, WordsApiService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IDeepLService, DeepLService>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<ITextToSpeechService, TextToSpeechService>();
            services.AddScoped<IRedisService, RedisService>();

            services.AddDbContext<WordpickerContext>(options =>
            {
                MySqlServerVersion mySqlServerVersion = new(new Version(Consts.MYSQL_VERSION));
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), mySqlServerVersion, opt => opt.CommandTimeout(120));
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
        }

        // Configure HTTP pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WordpickerContext context)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            // Environment setting
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            context.Database.Migrate();

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
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
