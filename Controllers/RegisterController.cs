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
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        private readonly EmpleadosContext db;

        public RegisterController(ILogger<RegisterController> logger, EmpleadosContext context)
        {
            _logger = logger;
            db = context;
        }

        public  IActionResult Register()
        {
            return View() ;
        }

        [HttpPost]
        public IActionResult Register(string email, string nombre)
        {
            Empresa nuevaEmpresa = new Empresa
            {
                Email = email,
                Nombre = nombre
            };

            db.Empresas.Add(nuevaEmpresa);
            db.SaveChanges();
            HttpContext.Session.Set<Empresa>("EmpresaLogueada", nuevaEmpresa);
            return Redirect("/Home/Index");

        }
    }
}