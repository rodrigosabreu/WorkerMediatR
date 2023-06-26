using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Queries;
using app.Application.Services;
using app.Domain.Entities;
using app.Domain.Services;
using app.Infrastructure.Context;
using app.Infrastructure.Repository;
using app.Worker;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        //logging.ClearProviders();
        logging.AddSerilog(new LoggerConfiguration()
            .WriteTo.EventCollector("http://localhost:8088/services/collector", "671153f3-d202-4c26-8e81-bbbdbeb5d1eb")
            .CreateLogger());
    })
    .ConfigureServices(services =>
    {
        IConfiguration config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
        .Build();
        
        // Registrar dependÍncias
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IEstruturaComercialRepository, EstruturaComercialRepository>();

        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IEstruturaComercialService, EstruturaComercialService>();
        services.AddTransient<ISqsService, SqsService>();
        services.AddTransient<IKafkaConsumerService, KafkaConsumerService>();        

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));        
        services.AddTransient<IRequestHandler<SendMessageCommand>, SendMessageCommandHandler>();
        services.AddTransient<IRequestHandler<ProcessMessageCommand>, ProcessMessageCommandHandler>();
        services.AddTransient<IRequestHandler<GetProductsQuery, IEnumerable<Product>>, GetProductsQueryHandler>();
        services.AddTransient<IRequestHandler<GetEstruturaComercialQuery>, GetEstruturaComercialQueryHandler>();
        

        services.AddDbContext<AppDbContext>(ServiceLifetime.Singleton);

        string connectionString = config.GetConnectionString("DefaultConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

        services.AddDbContext<EstruturaComercialContext>(
               dbContextOptions => dbContextOptions
                   .UseMySql(connectionString, serverVersion), ServiceLifetime.Singleton
        );

        services.AddMemoryCache();

    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
