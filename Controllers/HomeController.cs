﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpresaIT.Models;

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

        public IActionResult Index()
        {
            return View(db.Empleados.ToList());
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
