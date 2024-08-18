using Microsoft.AspNetCore.Mvc;

namespace Sistema_Nominas.Controllers
{
    public class GenerarNominaPDF : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
