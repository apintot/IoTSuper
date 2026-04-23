
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
    }
}
