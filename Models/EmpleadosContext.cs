using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace model
{
    public class EmpleadosContext : DbContext  
    {
        public NotasContext(DbContextOptions<EmpleadosContext> options)
         : base(options)
        {}

        public DbSet<Empleado> Empleados {get; set;}

        public DbSet<Empresa> Empresas {get; set;}


    }
}