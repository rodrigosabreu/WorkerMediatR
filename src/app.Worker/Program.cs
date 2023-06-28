using app.Application.Registrations;
using app.Infrastructure.Registrations;
using app.Worker;
using app.Worker.Registrations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        BootstrapWorker.RegisterLogger(logging);
    })
    .ConfigureServices(services =>
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        BootstrapApplication.RegisterApplication(services);
        BootstrapInfrastructure.RegisterInfrastructure(services, config);
        BootstrapWorker.RegisterWorker(services);
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
