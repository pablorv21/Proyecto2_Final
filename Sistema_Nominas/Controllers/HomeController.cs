using Microsoft.AspNetCore.Mvc;
using Sistema_Nominas.Models;
using System.Diagnostics;

namespace Sistema_Nominas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Empleado2()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Empleados", "Empleado2.aspx");
            if (System.IO.File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);
                return Content(content, "text/html");
            }
            return NotFound();
        }
        public IActionResult Index()
        {
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
