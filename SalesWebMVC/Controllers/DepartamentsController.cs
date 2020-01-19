using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;

namespace SalesWebMVC.Controllers
{
    public class DepartamentsController : Controller
    {
        public IActionResult Index()
        {
            List<Departament> list = new List<Departament>
            {
                new Departament() { Id = 1, Name = "Eletronics" },
                new Departament() { Id = 2, Name = "Fashion" }
            };
            
            return View(list);
        }
    }
}