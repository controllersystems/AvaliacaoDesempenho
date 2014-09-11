using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Models.Home;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            return View(model);
        }
    }
}