using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpresaIT.Models;
using System.Net.Mail;

namespace EmpresaIT.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly EmpleadosContext db;

        public AdminController(ILogger<AdminController> logger, EmpleadosContext empleadosContext)
        {
            _logger = logger;
            db = empleadosContext ;
        }

        public  IActionResult Index()
        {
            Empresa empresa = HttpContext.Session.Get<Empresa>("EmpresaLogueada");
            if ( empresa != null) {
                List<Empleado> empleadosList = new List<Empleado>();
                empleadosList = db.Empleados.Where(e => e.Empresa.Email.Equals(empresa.Email)).ToList() ;
                return View("Index",empleadosList);
            }
            else {
                return Redirect("/Login/AdminAccount");
            }
        }


        [HttpGet]
        public IActionResult Empleado(int id) {
            Empleado empleado = db.Empleados.FirstOrDefault(e => e.ID == id) ;
            this.ViewBag.NombreCompleto = empleado.NombreCompleto ;
            this.ViewBag.PuestoDeTrabajo = empleado.PuestoDeTrabajo ;
            this.ViewBag.Sueldo = empleado.Sueldo ;
            this.ViewBag.Edad = empleado.Edad ;
            this.ViewBag.Email = empleado.Email ;
            this.ViewBag.Sueldo = empleado.Sueldo ;
            return View();
        }

        [HttpPost]
        public IActionResult AgregarEmpleado(int id,string nombreCompleto, string email, int edad, string sexo, string puesto, int sueldo) 
        {
            Empresa empresa = HttpContext.Session.Get<Empresa>("EmpresaLogueada");
            if ( empresa != null) {

                Empresa empresaFind = db.Empresas.FirstOrDefault(e => e.Email.Equals(empresa.Email));
                Empleado nuevoEmpleado = new Empleado{
                ID = id,
                NombreCompleto = nombreCompleto,
                Email = email,
                Contraseña = nombreCompleto,
                Edad = edad,
                Sexo = sexo,
                PuestoDeTrabajo = puesto,
                Sueldo = sueldo,
                Empresa = empresaFind
                };

                
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress("grupo2comit@gmail.com");
                MailMessage message = new MailMessage(from, to);

                message.Subject = "Bienvenido a " + empresa.Nombre ;
                message.Body = "<h3>Hola " + nombreCompleto + "</h3>" +
                                "<div><p>Esperamos que te encuentres muy bien, te damos la bienvenida a " + empresa.Nombre + ". A continuacion te vamos a estar enviando una serie de formularios que necesitamos que completes con toda tu informacion.</p></div>"+
                                "<div><p>Para acceder a la aplicacion se te dará una cuenta , la cual posee un email y un nombre de usuario.</div></p>" +
                                "<div><p>Tus datos son los siguientes :</div></p> " +
                                "<ul><li><div><p>Email :" + email + "</li></div></p>" +
                                "<li><div><p>Contraseña :" + nombreCompleto + "</li></div></p></ul>" +
                                "<div><p>Estamos a disposicion por cualquier consulta que tengas.</p></div>"+
                                "<div><p>Saludos Cordiales</div></p>";
               

                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("grupo2comit@gmail.com", "comit1597"),
                    EnableSsl = true
                }; 

                try
                {  
                    client.Send(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                db.Empleados.Add(nuevoEmpleado);
                db.SaveChanges();
                return RedirectToAction("Index" ,"Admin");
            }
            
            else {
                return Json("No se puede agregar un empleado si no estas logueado");
            }

            
        }
        
         [HttpPost]
        public IActionResult Editar(int id,string nombreCompleto, string email,string contraseña, int edad, string sexo, string puesto, int sueldo)
        {
            Empleado empleadoEdit = db.Empleados.FirstOrDefault(e => e.ID == id);
            empleadoEdit.Email = email;
            empleadoEdit.Contraseña = contraseña;
            empleadoEdit.Edad = edad ;
            empleadoEdit.Sexo = sexo ;
            empleadoEdit.PuestoDeTrabajo = puesto ;
            empleadoEdit.Sueldo = sueldo ;

            db.Empleados.Update(empleadoEdit);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }


        public IActionResult EliminarEmpleado(int id)
        {
            Empleado empleado = db.Empleados.FirstOrDefault(e => e.ID == id);
            
            db.Empleados.Remove(empleado);
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult AgregarEmpleado()
        {
            return View() ;
        }

        public IActionResult SacarEmpresaEnSesion()
        {
            HttpContext.Session.Remove("EmpresaLogueada");
            return RedirectToAction("Index", "Home") ;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
