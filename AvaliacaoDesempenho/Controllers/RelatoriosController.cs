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
        public ActionResult Engajamento()
        {
            EngajamentoViewModel model = new EngajamentoViewModel();

            return View(model);
        }
        public ActionResult GapCompetencias()
        {
            GapCompetenciasViewModel model = new GapCompetenciasViewModel();

            return View(model);
        }
        public ActionResult RatingFinalColaborador()
        {
            RatingFinalColaboradorViewModel model = new RatingFinalColaboradorViewModel();

            return View(model);
        }
        public ActionResult RatingFinalGestor()
        {
            RatingFinalGestorViewModel model = new RatingFinalGestorViewModel();

            return View(model);
        }
        public ActionResult RatingFinalRH()
        {
            RatingFinalRHViewModel model = new RatingFinalRHViewModel();

            return View(model);
        }
        public ActionResult RelatorioPDI()
        {
            RelatorioPDIViewModel model = new RelatorioPDIViewModel();

            return View(model);
        }
        public ActionResult ReuniaoCalibragem()
        {
            ReuniaoCalibragemViewModel model = new ReuniaoCalibragemViewModel();

            return View(model);
        }
    }
}
