using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaIT.Models
{
    public class Empleado
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {get;set;}

        [Required]
        public string NombreCompleto {get;set;}

        [Required]
        public string Email {get;set;}

        [Required]
        public string PuestoDeTrabajo {get;set;}

        [Required]
        public int Sueldo {get;set;}
    }
}
