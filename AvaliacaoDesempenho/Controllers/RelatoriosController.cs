using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvaliacaoDesempenho.Models.Relatorios;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Rubi.Integracoes;

namespace AvaliacaoDesempenho.Controllers
{
    public class RelatoriosController : Controller
    {
        public ActionResult Index(int? cicloSelecionado)
        {
            RelatoriosViewModel model = new RelatoriosViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }

        public ActionResult AcompanhamentoGeral(int? cicloSelecionado)
        {
            AcompanhamentoGeralViewModel model = new AcompanhamentoGeralViewModel();

            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaAcompanhamentoGeral = new List<ItemAcompanhamentoGeralViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        model.ListaAcompanhamentoGeral.Add(new ItemAcompanhamentoGeralViewModel
                        {
                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            StatusAvaliacao = item.StatusAvaliacaoColaborador.Nome
                        });
                    }
                }
            }

            return View(model);
        }

        public ActionResult AcompanhamentoEtapa(int? cicloSelecionado)
        {
            AcompanhamentoEtapaViewModel model = new AcompanhamentoEtapaViewModel();

            model.CicloSelecionado = cicloSelecionado;

            return View(model);
        }

        public ActionResult Engajamento(int? cicloSelecionado)
        {
            EngajamentoViewModel model = new EngajamentoViewModel();

            model.CicloSelecionado = cicloSelecionado;

            return View(model);
        }
        public ActionResult GapCompetencias(int? cicloSelecionado)
        {
            GapCompetenciasViewModel model = new GapCompetenciasViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
        public ActionResult RatingFinalColaborador(int? cicloSelecionado)
        {
            RatingFinalColaboradorViewModel model = new RatingFinalColaboradorViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
        public ActionResult RatingFinalGestor(int? cicloSelecionado)
        {
            RatingFinalGestorViewModel model = new RatingFinalGestorViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
        public ActionResult RatingFinalRH(int? cicloSelecionado)
        {
            RatingFinalRHViewModel model = new RatingFinalRHViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
        public ActionResult RelatorioPDI(int? cicloSelecionado)
        {
            RelatorioPDIViewModel model = new RelatorioPDIViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
        public ActionResult ReuniaoCalibragem(int? cicloSelecionado)
        {
            ReuniaoCalibragemViewModel model = new ReuniaoCalibragemViewModel();
            model.CicloSelecionado = cicloSelecionado;
            return View(model);
        }
    }
}
