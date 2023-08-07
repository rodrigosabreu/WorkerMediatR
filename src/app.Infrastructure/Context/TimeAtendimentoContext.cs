using app.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Infrastructure.Context 
{
    public class TimeAtendimentoContext : DbContext
    {
        public DbSet<TimeAtendimentoPrivate_TBL> TimeAtendimentoPrivate_TBL { get; set; }

        public TimeAtendimentoContext(DbContextOptions<TimeAtendimentoContext> options) : base(options) { }
        //public DbSet<EstruturaComercial> EstruturaComercial { get; set; }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeAtendimentoPrivate_TBL>().ToTable("tbl_time_atendimento");
            modelBuilder.Entity<TimeAtendimentoPrivate_TBL>().HasKey(p => p.Id);
            modelBuilder.Entity<TimeAtendimentoPrivate_TBL>().Property(p => p.Cliente).HasColumnName("id_cliente");
            modelBuilder.Entity<TimeAtendimentoPrivate_TBL>().Property(p => p.Atendente).HasColumnName("id_especialista");            
        }
    }
}
