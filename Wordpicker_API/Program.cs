namespace Wordpicker_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostbuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostbuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5200");
                });
    }
}
