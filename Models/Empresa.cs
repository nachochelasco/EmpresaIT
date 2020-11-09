using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmpresaIT.Models
{
    public class Empresa
    {
        [Key]
        public int ID {get;set;}
        
        [Required]
        public string Nombre {get;set;}

        public List<Empleado> Empleados {get;set;}
    }
}