using MailerService;
using MailerService.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

//namespace MailerService
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args)
//        {
//            var host = Host.CreateDefaultBuilder(args)
//                .UseWindowsService()
//                .ConfigureServices((hostContext, services) =>
//                {
//                    var configuration = hostContext.Configuration;
//                    var optionsBuilder = new DbContextOptionsBuilder<TaskManagementContext>();
//                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));//,
//                    services.AddScoped<TaskManagementContext>(s => new TaskManagementContext(optionsBuilder.Options));

//                    services.AddHostedService<Worker>();
//                });

//            return host;
//        }
//    }
//}




Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().MinimumLevel
    .Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext().WriteTo.File("Logs/log.txt").CreateLogger();
var host = Host.CreateDefaultBuilder(args).UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        var optionsBuilder = new DbContextOptionsBuilder<TaskManagementContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));//,
        services.AddScoped<TaskManagementContext>(s => new TaskManagementContext(optionsBuilder.Options));

      

        services.AddHostedService<Worker>();
    }).UseSerilog()
    .Build();


try
{
    Log.Information("Starting Up service");
    await host.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Error Occurred in service");
}
finally
{
    Log.CloseAndFlush();
}


