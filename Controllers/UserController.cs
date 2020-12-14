using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpresaIT.Models;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore ;

namespace EmpresaIT.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly EmpleadosContext db;

        public UserController(ILogger<UserController> logger, EmpleadosContext empleadosContext)
        {
            _logger = logger;
            db = empleadosContext ;
        }

        public  IActionResult Index()
        {
            Empleado empleado = HttpContext.Session.Get<Empleado>("EmpleadoLogueado");
            Empresa empresaEmpleado = db.Empleados.Include(e => e.Empresa).FirstOrDefault(e => e.ID == empleado.ID).Empresa ;

            if ( empleado != null) {
                List<Empleado> empleadosList = new List<Empleado>();
                
                empleadosList = db.Empleados.Where(e => e.Empresa.Nombre.Equals(empresaEmpleado.Nombre)).ToList() ;   
                return View("Index",empleadosList);
            }
            else {
                return Redirect("/Login/UserAccount");
            }
        }

        [HttpGet]
        public IActionResult Empleado(int id) {
            Empleado empleado = db.Empleados.Find(id) ;
            this.ViewBag.NombreCompleto = empleado.NombreCompleto ;
            this.ViewBag.PuestoDeTrabajo = empleado.PuestoDeTrabajo ;
            this.ViewBag.Sueldo = empleado.Sueldo ;
            this.ViewBag.Edad = empleado.Edad ;
            this.ViewBag.Email = empleado.Email ;
            this.ViewBag.Sueldo = empleado.Sueldo ;
            return View();
        }
    }
}