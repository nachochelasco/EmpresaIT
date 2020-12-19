using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaIT.Models
{
    public class Empresa
    {
        [Key]
        [Required]
        public string Email {get;set;}
        
        [Required]
        public string Nombre {get;set;}

        [Required]
        public string Contraseña {get;set;}

        public List<Empleado> Empleados {get;set;}
    }
}