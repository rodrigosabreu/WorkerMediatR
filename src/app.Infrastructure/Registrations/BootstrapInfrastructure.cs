using app.Application.Interfaces;
using app.Infrastructure.Context;
using app.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app.Infrastructure.Registrations
{
    public static class BootstrapInfrastructure
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IEstruturaComercialRepository, EstruturaComercialRepository>();
            services.AddSingleton<ITimeAtendimentoPrivateRepository, TimeAtendimentoPrivateRepository>();

            string connectionString = config.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

            services.AddDbContext<AppDbContext>(ServiceLifetime.Singleton);
            services.AddDbContext<EstruturaComercialContext>(dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion), ServiceLifetime.Singleton
            );
            services.AddDbContext<TimeAtendimentoContext>(dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion), ServiceLifetime.Singleton
            );

            return services;
        }
    }
}
