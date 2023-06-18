using app.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Infrastructure.Context
{
    public class AppDbContext : DbContext { 
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("your-connection-string"); // Conexão com o banco de dados MySQL usando Pomelo.EntityFrameworkCore.MySql
        }
    }
}
