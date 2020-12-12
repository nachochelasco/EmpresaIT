﻿using System;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmpleadosContext db;

        public HomeController(ILogger<HomeController> logger, EmpleadosContext empleadosContext)
        {
            _logger = logger;
            db = empleadosContext ;
        }

        public  IActionResult Index()
        {
            Empresa empresa = HttpContext.Session.Get<Empresa>("EmpresaLogueada");
            if ( empresa != null) {
                List<Empleado> empleadosList = new List<Empleado>();
                empleadosList = db.Empleados.Where(e => e.Empresa.Nombre.Equals(empresa.Nombre)).ToList() ;
                return View("Index",empleadosList);
            }
            else {
                return Redirect("/Login/Account");
            }
        }


        [HttpGet]
        public IActionResult Empleado(int id) {
            Empleado empleado = db.Empleados.Find(id) ;
            this.ViewBag.NombreCompleto = empleado.NombreCompleto ;
            this.ViewBag.PuestoDeTrabajo = empleado.PuestoDeTrabajo ;
            this.ViewBag.Email = empleado.Email ;
            this.ViewBag.Sueldo = empleado.Sueldo ;
            return View();
        }

        [HttpPost]
        public IActionResult AgregarEmpleado(string nombreCompleto, string email, string puesto, int sueldo) 
        {
            Empresa empresa = HttpContext.Session.Get<Empresa>("EmpresaLogueada");
            if ( empresa != null) {

                Empresa empresaFind = db.Empresas.FirstOrDefault(e => e.Email.Equals(empresa.Email));
                Empleado nuevoEmpleado = new Empleado{
                NombreCompleto = nombreCompleto,
                Email = email,
                PuestoDeTrabajo = puesto,
                Sueldo = sueldo,
                Empresa = empresaFind
                };
                MailAddress to = new MailAddress(email);
                MailAddress from = new MailAddress("grupo2comit@gmail.com");
                MailMessage message = new MailMessage(from, to);

                message.Subject = "Bienvenido a " + empresa.Nombre ;
                message.Body = "Hola " + nombreCompleto + ". Esperamos que te encuentres muy bien, te damos la bienvenida a " + empresa.Nombre + ". A continuacion te vamos a estar enviando una serie de cuestionarios que necesitamos que completes con toda tu informacion. Estamos a disposicion por cualquier consulta que tengas.";

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
                return Redirect("/Home/Index");
            }
            
            else {
                return Json("No se puede agregar un empleado si no estas logueado");
            }

            
        }


        public IActionResult EliminarEmpleado(int ID)
        {
            Empleado empleado = db.Empleados.FirstOrDefault(e => e.ID == ID);
            
            db.Empleados.Remove(empleado);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AgregarEmpleado()
        {
            return View() ;
        }

        public IActionResult SacarEmpresaEnSesion()
        {
            HttpContext.Session.Remove("EmpresaLogueada");
            return RedirectToAction("Account", "Login") ;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
