using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpresaIT.Models;
using Microsoft.EntityFrameworkCore ;


namespace EmpresaIT.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        private readonly EmpleadosContext db;

        public LoginController(ILogger<LoginController> logger, EmpleadosContext context)
        {
            _logger = logger;
            db = context;
        }

        public JsonResult AgregarEmpresaALaSession(string email,string nombre)
        {
            Empresa nuevaEmpresa = new Empresa{
                Email = email,
                Nombre = nombre
            };

            HttpContext.Session.Set<Empresa>("EmpresaLogueada", nuevaEmpresa);
            return Json(nuevaEmpresa);
        }

        public IActionResult AdminAccount() {
            return View() ;
        }

        public IActionResult UserAccount() {
            return View() ;
        }

        [HttpPost]
        public  IActionResult AdminAccount(string email,string nombre) {
            {
            Empresa empresaCheck = db.Empresas.FirstOrDefault(u => u.Email == email && u.Nombre == nombre);
            if(empresaCheck != null) 
            {
                if(empresaCheck.Nombre == nombre) {
                    AgregarEmpresaALaSession(email,nombre) ;
                    return RedirectToAction("Index", "Admin");
                }
                else {
                    ViewBag.ErrorEnLogin = true;
                    return View("AdminAccount");
                }
            } else {
                ViewBag.ErrorEnLogin = true;
                return View("AdminAccount");
               }
            }
        }

        [HttpPost]
        public  IActionResult UserAccount(string email,string nombre) {
            {
            Empleado userCheck = db.Empleados.FirstOrDefault(e => e.Email == email && e.NombreCompleto == nombre);
            if(userCheck != null) 
            {
                if(userCheck.NombreCompleto == nombre) {
                    HttpContext.Session.Set<Empleado>("EmpleadoLogueado", userCheck);
                    return RedirectToAction("Index", "User");
                }
                else {
                    ViewBag.ErrorEnLogin = true;
                    return View("UserAccount");
                }
            } else {
                ViewBag.ErrorEnLogin = true;
                return View("UserAccount");
               }
            }
        }
    }
}
            