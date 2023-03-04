using Microsoft.EntityFrameworkCore;
using System.Dynamic;
//Se hace de nuevo porque me habia elminado esta clase.

namespace L01_2020MV603.Models
{
    public class infoContext : DbContext
    {
        public infoContext(DbContextOptions<infoContext> options) : base(options) 
        { 
        }

        public DbSet<calificaciones> calificaciones { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<publicaciones> publicaciones { get; set;}
        public DbSet<roles> roles { get; set; }
        public DbSet<usuarios> usuarios { get; set; }

    }
}
