using app.Application.LogWorker;
using app.Application.Registrations;
using app.Infrastructure.Registrations;
using app.Worker;
using app.Worker.Registrations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        ApplicationLogging.LoggerFactory = new LoggerFactory();
        BootstrapWorker.RegisterLogger(logging, ConfigBuilder());
    })
    .ConfigureServices((Action<IServiceCollection>)(services =>
    {        

        ApplicationLogging.LoggerFactory = new LoggerFactory();

        BootstrapApplication.RegisterApplication(services);
        BootstrapInfrastructure.RegisterInfrastructure(services, ConfigBuilder());
        BootstrapWorker.RegisterWorker(services);
    }))
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

static IConfiguration ConfigBuilder()
{
    return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
}