using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmpresaIT.Models
{
    public class EmpleadosContext : DbContext  
    {
        public EmpleadosContext(DbContextOptions<EmpleadosContext> options)
         : base(options)
        {}

        public DbSet<Empleado> Empleados {get; set;}

        public DbSet<Empresa> Empresas {get; set;}


    }
}