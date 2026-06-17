
using IoTSuper_API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Localizacion> Localizaciones { get; set; }

        public DbSet<Tipologia> Tipologias { get; set; }

        public DbSet<Centro> Centros { get; set; }

        public DbSet<Seccion> Secciones { get; set; }

        public DbSet<Componente> Componentes { get; set; }

        public DbSet<Evento> Eventos { get; set; }

        public DbSet<Termometro> Termometros { get; set; }

        public DbSet<Etiqueta> Etiquetas { get; set; }

        public DbSet<Stock> Stocks { get; set; }
    }
}
