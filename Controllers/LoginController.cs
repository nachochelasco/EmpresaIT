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

        public JsonResult AgregarEmpresaALaSession(string email,string contraseña)
        {
            Empresa nuevaEmpresa = new Empresa{
                Email = email,
                Contraseña = contraseña
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
        public  IActionResult AdminAccount(string email,string contraseña) {
            {
            Empresa empresaCheck = db.Empresas.FirstOrDefault(u => u.Email == email);
            if(empresaCheck != null) 
            {
                if(empresaCheck.Contraseña == contraseña) {
                    AgregarEmpresaALaSession(email,contraseña) ;
                    return RedirectToAction("Index", "Admin");
                }
                else {
                    ViewBag.ErrorEnLoginAdmin = true;
                    return View("AdminAccount");
                }
            } else {
                ViewBag.ErrorEnLoginAdmin = true;
                return View("AdminAccount");
               }
            }
        }

        [HttpPost]
        public  IActionResult UserAccount(string email,string contraseña) {
            {
            Empleado userCheck = db.Empleados.FirstOrDefault(e => e.Email == email);
            if(userCheck != null) 
            {
                if(userCheck.Contraseña == contraseña) {
                    HttpContext.Session.Set<Empleado>("EmpleadoLogueado", userCheck);
                    return RedirectToAction("Index", "User");
                }
                else {
                    ViewBag.ErrorEnLoginUser = true;
                    return View("UserAccount");
                }
            } else {
                ViewBag.ErrorEnLoginUser = true;
                return View("UserAccount");
               }
            }
        }
    }
}
            