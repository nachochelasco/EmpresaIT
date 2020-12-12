using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpresaIT.Models;


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

        public IActionResult Account() {
            return View() ;
        }

        [HttpPost]
        public  IActionResult Account(string email,string nombre) {
            {
            Empresa empresaCheck = db.Empresas.FirstOrDefault(u => u.Email == email);
            if(empresaCheck != null) 
            {
                if(empresaCheck.Nombre == nombre) {
                    AgregarEmpresaALaSession(email,nombre) ;
                    return RedirectToAction("Index", "Home");
                }
                else {
                    return Redirect("/Login/Account");
                }
            } else {
                return Redirect("/Login/Account");
             }
            }
        }
    }
}
            