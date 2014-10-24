using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvaliacaoDesempenho.Models.Relatorios;

namespace AvaliacaoDesempenho.Controllers
{
    public class RelatoriosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcompanhamentoGeral()
        {
            AcompanhamentoGeralViewModel model = new AcompanhamentoGeralViewModel();

            return View(model);
        }

        public ActionResult AcompanhamentoEtapa()
        {
            AcompanhamentoEtapaViewModel model = new AcompanhamentoEtapaViewModel();

            return View(model);
        }
    }
}
