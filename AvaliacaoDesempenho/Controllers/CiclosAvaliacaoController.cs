using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Integracoes.Rubi.Contratos;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Rubi.Integracoes;
using AvaliacaoDesempenho.Util.Mapeamentos;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Controllers
{
    public class CiclosAvaliacaoController : Controller
    {
        #region Gestão de Ciclos de Avaliação

        [HttpGet]
        [Authorize]
        [CriacaoMapeamento(typeof(DeCicloAvaliacaoParaItemListaGestaoCicloAvaliacaoViewModel))]
        public ActionResult GestaoCiclosAvaliacao()
        {
            GestaoCiclosAvaliacaoViewModel model = new GestaoCiclosAvaliacaoViewModel();

            model.CiclosAvaliacao
                = Mapper.Map<List<CicloAvaliacao>,
                             List<ItemListaGestaoCicloAvaliacaoViewModel>>(new CicloAvaliacaoDAO().Listar());

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [CriacaoMapeamento(typeof(DeCicloAvaliacaoParaManterCicloAvaliacaoViewModel))]
        [CriacaoMapeamento(typeof(DeSituacaoCicloAvaliacaoParaSelectListItem))]
        public ActionResult ManterCicloAvaliacao(int? id, Enumeradores.AcaoPagina acaoPagina = Enumeradores.AcaoPagina.Criar)
        {
            ManterCicloAvaliacaoViewModel model = new ManterCicloAvaliacaoViewModel();

            if (id.HasValue)
            {
                var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

                model = Mapper.Map<CicloAvaliacao,
                                   ManterCicloAvaliacaoViewModel>(cicloAvaliacao);

                if (model == null)
                    return HttpNotFound();
                else
                    model.SituacaoCicloAvaliacaoSelecionadoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID.Value;
            }

            model.AcaoPagina = acaoPagina;

            model.SituacoesCicloAvaliacao
                = Mapper.Map<List<SituacaoCicloAvaliacao>,
                             List<SelectListItem>>(new SituacaoCicloAvaliacaoDAO().Listar());

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeManterCicloAvaliacaoViewModelParaCicloAvaliacao))]
        [CriacaoMapeamento(typeof(DeSituacaoCicloAvaliacaoParaSelectListItem))]
        public ActionResult ManterCicloAvaliacao(ManterCicloAvaliacaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                CicloAvaliacaoDAO cicloAvaliacaoDAO = new CicloAvaliacaoDAO();

                var cicloAvaliacao
                    = Mapper.Map<ManterCicloAvaliacaoViewModel,
                                 CicloAvaliacao>(model);

                cicloAvaliacao.SituacaoCicloAvaliacao_ID = model.SituacaoCicloAvaliacaoSelecionadoID;

                if (!model.ID.HasValue)
                    cicloAvaliacaoDAO.Incluir(cicloAvaliacao);
                else
                    cicloAvaliacaoDAO.Editar(cicloAvaliacao);

                return RedirectToAction("GestaoCiclosAvaliacao");
            }

            model.SituacoesCicloAvaliacao
                = Mapper.Map<List<SituacaoCicloAvaliacao>,
                             List<SelectListItem>>(new SituacaoCicloAvaliacaoDAO().Listar());

            return View(model);
        }

        #endregion Gestão de Ciclos de Avaliação

        #region Gestão de Cargos e Competências

        [HttpGet]
        [Authorize]
        [CriacaoMapeamento(typeof(DeAssociacaoCargoCompetenciaParaItemListaGestaoCompetenciasCargosViewModel))]
        [CriacaoMapeamento(typeof(DeUSU_V092ESTParaAssociacaoParaAssociacaoCargoCompetenciaViewModel))]
        [CriacaoMapeamento(typeof(Detbl_cargo_sccParaSelectListItem))]
        [CriacaoMapeamento(typeof(Detbl_area_sccParaSelectListItem))]
        [CriacaoMapeamento(typeof(Detbl_setor_sccParaSelectListItem))]
        public ActionResult GestaoCompetenciasCargos(int? id)
        {
            GestaoCompentenciasCargosViewModel model = new GestaoCompentenciasCargosViewModel();

            model.CicloAvaliacaoSelecionadoID = id;

            CarregarAssociacoesCargoCompetencias(model);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ConsolidarCargosRubi(int? id)
        {
            GestaoCompentenciasCargosViewModel model = new GestaoCompentenciasCargosViewModel();

            model.CicloAvaliacaoSelecionadoID = id;

            CarregarInformacoesRubi(model);

            return View("/Views/CiclosAvaliacao/GestaoCompetenciasCargos.cshtml", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeItemListaGestaoCompetenciasCargosViewModelParaAssociacaoCargoCompetencia))]
        public ActionResult GestaoCompetenciasCargos(GestaoCompentenciasCargosViewModel model)
        {
            if (ModelState.IsValid)
            {
                AssociacaoCargoCompetenciaDAO associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

                CarregarInformacoesSistemaCompentencias(model);

                foreach (var item in model.AssociacoesCargosCompetencias)
                {
                    if (item.CargoCompetenciaID.HasValue)
                        item.CargoCompetencia = model.CargosCompentencia.First(p => p.Value.Equals(item.CargoCompetenciaID.Value.ToString())).Text;

                    if (item.AreaCompetenciaID.HasValue)
                        item.AreaCompetencia = model.AreasCompentencia.First(p => p.Value.Equals(item.AreaCompetenciaID.ToString())).Text;

                    if (item.SetorCompetenciaID.HasValue)
                        item.SetorCompetencia = model.SetoresCompentencia.First(p => p.Value.Equals(item.SetorCompetenciaID.ToString())).Text;

                    item.CicloAvaliacaoID = model.CicloAvaliacaoSelecionadoID.Value;
                }

                var associacoesCargoCompetencia = Mapper.Map<List<ItemListaGestaoCompetenciasCargosViewModel>, 
                                                             List<AssociacaoCargoCompetencia>>(model.AssociacoesCargosCompetencias);

                associacaoCargoCompetenciaDAO.PersistirColecao(associacoesCargoCompetencia);
            }

            return View(model);
        }

        private void CarregarAssociacoesCargoCompetencias(GestaoCompentenciasCargosViewModel model)
        {
            model.AssociacoesCargosCompetencias =
                Mapper.Map<List<AssociacaoCargoCompetencia>,
                           List<ItemListaGestaoCompetenciasCargosViewModel>>
                                (new AssociacaoCargoCompetenciaDAO().
                                    ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID.Value));

            if (!model.AssociacoesCargosCompetencias.Any())
                CarregarInformacoesRubi(model);

            CarregarInformacoesSistemaCompentencias(model);
        }

        #region Carga das informações do Rubi

        private void CarregarInformacoesRubi(GestaoCompentenciasCargosViewModel model)
        {
            model.AssociacoesCargosCompetencias =
                Mapper.Map<List<USU_V092EST>,
                           List<ItemListaGestaoCompetenciasCargosViewModel>>
                                (new IntegracaoRubi().ListarUSU_V092EST());
        }

        #endregion Carga das informações do Rubi

        #region Carga das informações do Sistema de Competências

        private void CarregarInformacoesSistemaCompentencias(GestaoCompentenciasCargosViewModel model)
        {
            CarregarCargosSistemaCompentencias(model);
            CarregarAreasSistemaCompentencias(model);
            CarregarSetoresSistemaCompentencias(model);
        }

        private void CarregarCargosSistemaCompentencias(GestaoCompentenciasCargosViewModel model)
        {
            model.CargosCompentencia =
                Mapper.Map<List<tbl_cargo_scc>, List<SelectListItem>>
                    (new IntegracaoSistemaCompetencias().ListarCargosCompetencias());
        }

        private void CarregarAreasSistemaCompentencias(GestaoCompentenciasCargosViewModel model)
        {
            model.AreasCompentencia =
                Mapper.Map<List<tbl_area_scc>, List<SelectListItem>>
                    (new IntegracaoSistemaCompetencias().ListarAreasCompetencias());
        }

        private void CarregarSetoresSistemaCompentencias(GestaoCompentenciasCargosViewModel model)
        {
            model.SetoresCompentencia =
                Mapper.Map<List<tbl_setor_scc>, List<SelectListItem>>
                    (new IntegracaoSistemaCompetencias().ListarSetoresCompetencias());
        }

        #endregion Carga das informações do Sistema de Competências

        #endregion Gestão de Cargos e Competências

        #region Views Parciais

        [Authorize]
        [CriacaoMapeamento(typeof(DeCicloAvaliacaoParaSelectListItem))]
        public ActionResult SelecaoCicloAvaliacao(int? cicloAvaliacaoSelecionadoID)
        {
            SelecaoCicloAvaliacaoViewModel model = new SelecaoCicloAvaliacaoViewModel();

            model.CiclosAvaliacao
                = Mapper.Map<List<CicloAvaliacao>,
                             List<SelectListItem>>(new CicloAvaliacaoDAO().Listar());

            if (cicloAvaliacaoSelecionadoID.HasValue)
                model.CicloAvaliacaoSelecionadoID = cicloAvaliacaoSelecionadoID.Value;

            return PartialView(model);
        }

        [Authorize]
        [CriacaoMapeamento(typeof(DeCicloAvaliacaoParaCicloAvaliacaoSelecionadoViewModel))]
        public ActionResult CicloAvaliacaoSelecionado(int? cicloAvaliacaoSelecionadoID)
        {
            CicloAvaliacaoSelecionadoViewModel model = new CicloAvaliacaoSelecionadoViewModel();

            model = Mapper.Map<CicloAvaliacao,
                               CicloAvaliacaoSelecionadoViewModel>
                                    (new CicloAvaliacaoDAO().Obter(cicloAvaliacaoSelecionadoID.Value));

            return PartialView(model);
        }

        #endregion Views Parciais

        [HttpGet]
        public ActionResult EnvioEmails()
        {
            return View();
        }
    }
}