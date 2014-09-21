using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Seguranca;
using AvaliacaoDesempenho.Util.Mapeamentos;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Controllers
{
    public class AvaliacoesController : Controller
    {
        #region Gestão avaliações colaboradores de um ciclo

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeStatusAvaliacaoColaboradorParaSelectListItem))]
        public ActionResult GestaoAvaliacoesColaboradores(int? id)
        {
            GestaoAvaliacoesColaboradoresViewModel model = new GestaoAvaliacoesColaboradoresViewModel();

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return CarregarAvaliacoesColaboradoresPorCiclo(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarAvaliacoes(GestaoAvaliacoesColaboradoresViewModel model)
        {
            return CarregarAvaliacoesColaboradoresPorCiclo(model);
        }

        private ActionResult CarregarAvaliacoesColaboradoresPorCiclo(GestaoAvaliacoesColaboradoresViewModel model)
        {
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloAvaliacaoSelecionadoID);
            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);
            var gestoresCicloAvaliacao = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);

            model.AvaliacoesColaboradores =
                (from avaliacao in avaliacoes
                 join gestor in gestoresCicloAvaliacao
                    on avaliacao.GestorRubiID equals gestor.UsuarioRubiID
                 join associacao in associacoesCargosCompetencias
                        on new
                        {
                            CargoRubiID = avaliacao.CargoRubiID.Value,
                            AreaRubiID = avaliacao.AreaRubiID.Value,
                            SetorRubiID = avaliacao.SetorRubiID
                        }
                        equals new
                        {
                            CargoRubiID = associacao.CargoRubiID,
                            AreaRubiID = associacao.AreaRubiID,
                            SetorRubiID = associacao.SetorRubiID
                        }
                 where avaliacao.Usuario.Nome.ToUpper()
                                    .Contains(!string.IsNullOrEmpty(model.NomeAvaliadoPesquisado)
                                              && !string.IsNullOrWhiteSpace(model.NomeAvaliadoPesquisado)
                                                    ? model.NomeAvaliadoPesquisado.Trim().ToUpper()
                                                    : avaliacao.Usuario.Nome)
                    && associacao.AreaRubi.ToUpper()
                                    .Contains(!string.IsNullOrEmpty(model.AreaPesquisada)
                                              && !string.IsNullOrWhiteSpace(model.AreaPesquisada)
                                                    ? model.AreaPesquisada.Trim().ToUpper()
                                                    : associacao.AreaRubi.ToUpper())
                    && associacao.CargoRubi.ToUpper()
                                    .Contains(!string.IsNullOrEmpty(model.CargoPesquisado)
                                              && !string.IsNullOrWhiteSpace(model.CargoPesquisado)
                                                    ? model.CargoPesquisado.Trim().ToUpper()
                                                    : associacao.CargoRubi.ToUpper())
                    && gestor.Nome.ToUpper()
                                    .Contains(!string.IsNullOrEmpty(model.GestorPesquisado)
                                              && !string.IsNullOrWhiteSpace(model.GestorPesquisado)
                                                    ? model.GestorPesquisado.Trim().ToUpper()
                                                    : gestor.Nome.ToUpper())
                    && avaliacao.StatusAvaliacaoColaborador_ID
                                    .Equals(model.StatusAvaliacaoPesquisadoID.HasValue
                                                    ? model.StatusAvaliacaoPesquisadoID.Value
                                                    : avaliacao.StatusAvaliacaoColaborador_ID)
                 select new ItemListaGestaoAvaliacoesViewModel()
                 {
                     ID = avaliacao.ID,
                     NomeGestor = gestor.Nome,
                     UsuarioNome = avaliacao.Usuario.Nome,
                     Cargo = associacao.CargoRubi,
                     Area = associacao.AreaRubi,
                     StatusAvaliacaoColaboradorNome = avaliacao.StatusAvaliacaoColaborador.Nome,
                     StatusAvaliacaoColaboradorID = avaliacao.StatusAvaliacaoColaborador_ID
                 }).ToList();

            model.StatusAvaliacaoColaborador
                = Mapper.Map<List<StatusAvaliacaoColaborador>,
                             List<SelectListItem>>(new StatusAvaliacaoColaboradorDAO().Listar());

            return View("~/Views/Avaliacoes/GestaoAvaliacoesColaboradores.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AlterarStatusAvaliacoes(GestaoAvaliacoesColaboradoresViewModel model)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacoesCiclo = avaliacaoColaboradorDAO.Listar(model.CicloAvaliacaoSelecionadoID);

            var avaliacoesStatusAtualizado =
                (from avaliacao in avaliacoesCiclo
                 join avaliacaoAlteracao in model.AvaliacoesColaboradores
                    on avaliacao.ID equals avaliacaoAlteracao.ID
                 where avaliacao.StatusAvaliacaoColaborador_ID != avaliacaoAlteracao.StatusAvaliacaoColaboradorID
                 select new AvaliacaoColaborador()
                 {
                     Aprovada = avaliacao.Aprovada,
                     AreaRubiID = avaliacao.AreaRubiID,
                     CargoRubiID = avaliacao.CargoRubiID,
                     CicloAvaliacao_ID = avaliacao.CicloAvaliacao_ID,
                     Colaborador_ID = avaliacao.Colaborador_ID,
                     DataCriacao = avaliacao.DataCriacao,
                     GestorRubiID = avaliacao.GestorRubiID,
                     ID = avaliacao.ID,
                     JustificativaReprovacao = avaliacao.JustificativaReprovacao,
                     SetorRubiID = avaliacao.SetorRubiID,
                     StatusAvaliacaoColaborador_ID = avaliacaoAlteracao.StatusAvaliacaoColaboradorID
                 }).ToList();

            if (avaliacoesStatusAtualizado != null
                && avaliacoesStatusAtualizado.Any())
            {
                foreach (var avaliacao in avaliacoesStatusAtualizado)
                    avaliacaoColaboradorDAO.Editar(avaliacao);
            }

            return GestaoAvaliacoesColaboradores(model.CicloAvaliacaoSelecionadoID);
        }

        #endregion Gestão avaliações colaboradores de um ciclo

        [Authorize]
        [HttpGet]
        public ActionResult GestaoPDIs()
        {
            GestaoPDIsViewModel model = new GestaoPDIsViewModel();

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetImage(int colaboradorID)
        {
            Identidade usuario = new Identidade(colaboradorID);

            if (usuario.FOTEMP == null)
            {
                return File("~/Content/Images/sem_img.jpg", "image/jpg");
            }

            return File(usuario.FOTEMP, "image/jpg");
        }

        [Authorize]
        [HttpPost]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaGestaoAvaliacaoColaboradorViewModel))]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaItemListaGestaoAvaliacaoColaboradorViewModel))]
        public ActionResult GestaoAvaliacaoColaborador(SelecaoCicloAvaliacaoViewModel modelSelecao,
                                                       [ModelBinder(typeof(IdentidadeModelBinder))] Identidade identidade)
        {
            return CarregarGestaoAvaliacaoColaborador(modelSelecao.CicloAvaliacaoSelecionadoID, identidade);
        }

        private ActionResult CarregarGestaoAvaliacaoColaborador(int? cicloAvaliacaoSelecionadoID,
                                                                Identidade identidade)
        {
            GestaoAvaliacaoColaboradorViewModel model = new GestaoAvaliacaoColaboradorViewModel();

            var avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();
            var avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();
            var associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

            var avaliacaoColaborador = avaliacaoColaboradorDAO.Obter(cicloAvaliacaoSelecionadoID.Value, identidade.UsuarioID);

            if (avaliacaoColaborador != null)
            {
                model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;
                model.DataCriacao = avaliacaoColaborador.DataCriacao.ToShortDateString();
                model.StatusAvaliacaoColaboradorNome = avaliacaoColaborador.StatusAvaliacaoColaborador.Nome;
            }

            var avaliacaoPDI = avaliacaoPDIColaboradorDAO.Obter(cicloAvaliacaoSelecionadoID.Value, identidade.UsuarioID);

            if (avaliacaoPDI != null)
            {
                model.AvaliacaoPDIColaboradorID = avaliacaoPDI.ID;
                model.DataCriacaoPDI = avaliacaoPDI.DataCriacao.ToShortDateString();
                model.StatusPDINome = avaliacaoPDI.StatusPDI.Nome;
            }

            var avaliacoesCicloGestor = avaliacaoColaboradorDAO.ListarPorGestor(cicloAvaliacaoSelecionadoID.Value,
                                                                                identidade.UsuarioRubiID.Value);

            var associacoesCargosCompetencias = associacaoCargoCompetenciaDAO.ListarPorCicloAvaliacao(cicloAvaliacaoSelecionadoID.Value);

            if (avaliacoesCicloGestor != null
                && associacoesCargosCompetencias != null)
            {
                model.ListaGestaoAvaliacaoColaboradorViewModel =
                    (from avaliacao in avaliacoesCicloGestor
                     join associacao in associacoesCargosCompetencias
                        on new
                        {
                            CargoRubiID = avaliacao.CargoRubiID.Value,
                            AreaRubiID = avaliacao.AreaRubiID.Value,
                            SetorRubiID = avaliacao.SetorRubiID
                        }
                        equals new
                        {
                            CargoRubiID = associacao.CargoRubiID,
                            AreaRubiID = associacao.AreaRubiID,
                            SetorRubiID = associacao.SetorRubiID
                        }
                     select new ItemListaGestaoAvaliacaoColaboradorViewModel()
                     {
                         DataCriacao = avaliacao.DataCriacao.ToShortDateString(),
                         UsuarioID = avaliacao.Colaborador_ID,
                         UsuarioNome = avaliacao.Usuario.Nome,
                         Cargo = associacao.CargoRubi,
                         StatusAvaliacaoColaboradorNome = avaliacao.StatusAvaliacaoColaborador.Nome
                     }).ToList();
            }

            model.CicloAvaliacaoSelecionadoID = cicloAvaliacaoSelecionadoID;

            return View("~/Views/Avaliacoes/GestaoAvaliacaoColaborador.cshtml", model);
        }

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaViewModel))]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult RotearManutencaoAvaliacaoColaborador(int? id, int? colaboradorID = null, int? etapaAutoAvaliacao = (int)Enumeradores.EtapasAutoAvaliacao.AutoAvaliacao)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = new Identidade().UsuarioID;

            var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

            //if (avaliacaoColaborador.StatusAvaliacaoColaborador_ID.Equals((int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosGestores))
            //{

            //}
            //else
            if (avaliacaoColaborador == null
                || avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.DefinicaoObjetivosMetas)
                || avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.AprovacaoDefinicaoObjetivosMetas)
                || avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.ObjetivosMetasDefinidos))
            {
                return ManterAvaliacaoColaboradorObjetivosMetas(id, false, colaboradorID);
            }
            else if (avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.AutoAvaliacao))
            {
                if (etapaAutoAvaliacao.HasValue)
                {
                    switch (etapaAutoAvaliacao.Value)
                    {
                        case ((int)Enumeradores.EtapasAutoAvaliacao.AutoAvaliacao):
                            return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);

                        case ((int)Enumeradores.EtapasAutoAvaliacao.Competencias):
                            return ManterAvaliacaoColaboradorCompetencias(id, colaboradorID);

                        default:
                            return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                    }
                }
                else
                    return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
            }
            else if (avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosGestores))
            {
                return ManterAvaliacaoColaboradorAutoAvaliacao(id, true, true, colaboradorID);
            }
            return RedirectToAction("Index", "Home");
        }

        #region Objetivos e metas

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaViewModel))]
        public ActionResult ManterAvaliacaoColaboradorObjetivosMetas(int? id,
                                                                     bool incluirMeta = false,
                                                                     int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorObjetivosMetasViewModel model =
                new ManterAvaliacaoColaboradorObjetivosMetasViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
            {
                usuarioID = colaboradorID.Value;
                model.Aprovar = true;
            }
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

            if (avaliacaoColaborador != null)
            {
                model.GestorRubiID = avaliacaoColaborador.GestorRubiID;

                model.ListaObjetivosMetasViewModel =
                    Mapper.Map<List<ObjetivoColaborador>,
                               List<ObjetivoMetaViewModel>>
                                    (new ObjetivoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                model.Aprovada = avaliacaoColaborador.Aprovada;

                model.JustificativaReprovacao = avaliacaoColaborador.JustificativaReprovacao;
            }
            else
                model.GestorRubiID = identidade.GestorRubiID;

            model.CicloAvaliacaoSelecionadoID = id.Value;
            model.IncluirMeta = incluirMeta;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorObjetivosMetas.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaViewModel))]
        public ActionResult ManterAvaliacaoColaboradorObjetivosMetas(ManterAvaliacaoColaboradorObjetivosMetasViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.AvaliacaoColaboradorID.HasValue)
                {
                    AvaliacaoColaborador avaliacao = new AvaliacaoColaborador();

                    AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

                    var identidade = new Identidade();

                    avaliacao.DataCriacao = DateTime.Today;
                    avaliacao.CicloAvaliacao_ID = model.CicloAvaliacaoSelecionadoID.Value;
                    avaliacao.Colaborador_ID = identidade.UsuarioID;
                    avaliacao.GestorRubiID = identidade.GestorRubiID;
                    avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.DefinicaoObjetivosMetas;
                    avaliacao.CargoRubiID = identidade.CargoRubiID;
                    avaliacao.AreaRubiID = identidade.AreaRubiID;
                    avaliacao.SetorRubiID = identidade.SetorRubiID;

                    avaliacaoColaboradorDAO.Incluir(avaliacao);

                    model.AvaliacaoColaboradorID = avaliacao.ID;
                }

                ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();

                if (model.ObjetivoMetaCadastro != null)
                {
                    ObjetivoColaborador objetivo = new ObjetivoColaborador();

                    objetivo.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                    objetivo.Objetivo = model.ObjetivoMetaCadastro.Objetivo;

                    objetivo.MetaColaborador = new MetaColaborador();
                    objetivo.MetaColaborador.Meta = model.ObjetivoMetaCadastro.MetaColaboradorMeta;

                    objetivoColaboradorDAO.Incluir(objetivo);
                }
                else
                {
                    foreach (var item in model.ListaObjetivosMetasViewModel)
                    {
                        var objetivo = objetivoColaboradorDAO.Obter(item.ID);

                        objetivo.Objetivo = item.Objetivo;
                        objetivo.MetaColaborador.Meta = item.MetaColaboradorMeta;

                        objetivoColaboradorDAO.Editar(objetivo);
                    }
                }
            }

            return ManterAvaliacaoColaboradorObjetivosMetas(model.CicloAvaliacaoSelecionadoID);
        }

        [Authorize]
        public ActionResult DeletarAvaliacaoColaboradorObjetivosMetas(int? objetivoSelecionadoID,
                                                                      int? cicloAvaliacaoSelecionadoID)
        {
            if (ModelState.IsValid)
            {
                if (objetivoSelecionadoID.HasValue)
                {
                    ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();

                    objetivoColaboradorDAO.Excluir(objetivoSelecionadoID.Value);
                }
            }

            return ManterAvaliacaoColaboradorObjetivosMetas(cicloAvaliacaoSelecionadoID);
        }

        [Authorize]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaGestaoAvaliacaoColaboradorViewModel))]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaItemListaGestaoAvaliacaoColaboradorViewModel))]
        public ActionResult SubmeterAvaliacaoColaboradorAvaliacaoGestor(int? cicloAvaliacaoSelecionadoID,
                                                                        int? avaliacaoColaboradorObjetivosMetasID,
                                                                        [ModelBinder(typeof(IdentidadeModelBinder))] Identidade identidade)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacaoColaborador = avaliacaoColaboradorDAO.Obter(avaliacaoColaboradorObjetivosMetasID.Value);

            avaliacaoColaborador.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.AprovacaoDefinicaoObjetivosMetas;

            avaliacaoColaboradorDAO.Editar(avaliacaoColaborador);

            return CarregarGestaoAvaliacaoColaborador(cicloAvaliacaoSelecionadoID,
                                                      identidade);
        }

        [Authorize]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaGestaoAvaliacaoColaboradorViewModel))]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaItemListaGestaoAvaliacaoColaboradorViewModel))]
        public ActionResult SubmeterAutoAvaliacaoColaboradorAvaliacaoGestor(int? cicloAvaliacaoSelecionadoID,
                                                                            int? avaliacaoColaboradorID,
                                                                            [ModelBinder(typeof(IdentidadeModelBinder))] Identidade identidade)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacaoColaborador = avaliacaoColaboradorDAO.Obter(avaliacaoColaboradorID.Value);

            avaliacaoColaborador.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosGestores;

            avaliacaoColaboradorDAO.Editar(avaliacaoColaborador);

            return CarregarGestaoAvaliacaoColaborador(cicloAvaliacaoSelecionadoID,
                                                      identidade);
        }

        [Authorize]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaGestaoAvaliacaoColaboradorViewModel))]
        [CriacaoMapeamento(typeof(DeAvaliacaoColaboradorParaItemListaGestaoAvaliacaoColaboradorViewModel))]
        public ActionResult SubmeterPDIColaboradorAvaliacaoGestor(int? cicloAvaliacaoSelecionadoID,
                                                                  [ModelBinder(typeof(IdentidadeModelBinder))] Identidade identidade)
        {
            AvaliacaoPDIColaboradorDAO avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();

            var avaliacaoPDIColaborador = avaliacaoPDIColaboradorDAO.Obter(cicloAvaliacaoSelecionadoID.Value, identidade.UsuarioID);

            avaliacaoPDIColaborador.StatusPDI = new StatusPDIDAO().Obter((int)Enumeradores.StatusPDI.AprovacaoGestor);
            avaliacaoPDIColaborador.StatusPDI_ID = (int)Enumeradores.StatusPDI.AprovacaoGestor;

            avaliacaoPDIColaboradorDAO.Editar(avaliacaoPDIColaborador);

            return CarregarGestaoAvaliacaoColaborador(cicloAvaliacaoSelecionadoID,
                                                      identidade);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AprovarAvaliacaoColaborador(int? cicloAvaliacaoSelecionadoID, int? avaliacaoColaboradorObjetivosMetasID)
        {
            return AprovarReprovarAvaliacaoColaborador(cicloAvaliacaoSelecionadoID, avaliacaoColaboradorObjetivosMetasID, true, string.Empty);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReprovarAvaliacaoColaborador(ManterAvaliacaoColaboradorObjetivosMetasViewModel model)
        {
            return AprovarReprovarAvaliacaoColaborador(model.CicloAvaliacaoSelecionadoID, model.AvaliacaoColaboradorID, false, model.JustificativaReprovacao);
        }

        private ActionResult AprovarReprovarAvaliacaoColaborador(int? cicloAvaliacaoSelecionadoID,
                                                                 int? avaliacaoColaboradorObjetivosMetasID,
                                                                 bool aprovada,
                                                                 string justificativaReprovacao)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacao = avaliacaoColaboradorDAO.Obter(avaliacaoColaboradorObjetivosMetasID.Value);

            avaliacao.Aprovada = aprovada;

            if (!aprovada)
            {
                avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.DefinicaoObjetivosMetas;
                avaliacao.JustificativaReprovacao = justificativaReprovacao;
            }
            else
            {
                avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.ObjetivosMetasDefinidos;
                avaliacao.JustificativaReprovacao = string.Empty;
            }

            avaliacaoColaboradorDAO.Editar(avaliacao);

            return CarregarGestaoAvaliacaoColaborador(cicloAvaliacaoSelecionadoID, new Identidade());
        }

        #endregion Objetivos e metas

        #region Auto-avaliação

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorAutoAvaliacao(int? id,
                                                                    bool incluirMeta = false,
                                                                    bool incluirContribuicao = false,
                                                                    int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model =
                new ManterAvaliacaoColaboradorAutoAvaliacaoViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

            if (avaliacaoColaborador != null)
            {
                model.GestorRubiID = avaliacaoColaborador.GestorRubiID;

                model.ListaObjetivosMetasResultadosatingidosViewModel =
                    Mapper.Map<List<ObjetivoColaborador>,
                               List<ObjetivoMetaResultadoAtingidoViewModel>>
                                    (new ObjetivoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                model.ListaOutrasContribuicoesViewModel =
                    Mapper.Map<List<ContribuicaoColaborador>,
                               List<OutrasContribuicoesViewModel>>
                                   (new ContribuicaoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                model.ListaAvaliacaoGestor = new List<Models.Avaliacoes.AvaliacaoGestor>();
                model.ListaAvaliacaoGestor.Add(new Models.Avaliacoes.AvaliacaoGestor { ID = 0, Avaliacao = string.Empty  });

                model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;
            }
            else
                model.GestorRubiID = identidade.GestorRubiID;

            model.CicloAvaliacaoSelecionadoID = id.Value;
            model.IncluirMeta = incluirMeta;
            model.IncluirContribuicao = incluirContribuicao;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorAutoAvaliacao.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManterAvaliacaoColaboradorAutoAvaliacao(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();
                ContribuicaoColaboradorDAO contribuicaoColaboradorDAO = new ContribuicaoColaboradorDAO();

                if (model.ObjetivoMetaResultadoAtingidoCadastro != null)
                {
                    ObjetivoColaborador objetivo = new ObjetivoColaborador();

                    objetivo.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                    objetivo.Objetivo = model.ObjetivoMetaResultadoAtingidoCadastro.Objetivo;
                    objetivo.AutoAvaliacao = true;

                    objetivo.MetaColaborador = new MetaColaborador();
                    objetivo.MetaColaborador.Meta = model.ObjetivoMetaResultadoAtingidoCadastro.MetaColaboradorMeta;

                    objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
                    objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido
                        = model.ObjetivoMetaResultadoAtingidoCadastro.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;

                    objetivoColaboradorDAO.Incluir(objetivo);
                }
                else if (model.ListaObjetivosMetasResultadosatingidosViewModel != null)
                {
                    foreach (var item in model.ListaObjetivosMetasResultadosatingidosViewModel)
                    {
                        var objetivo = objetivoColaboradorDAO.Obter(item.ID);

                        objetivo.Objetivo = item.Objetivo;
                        objetivo.MetaColaborador.Meta = item.MetaColaboradorMeta;

                        if (objetivo.MetaColaborador.ResultadoAtingidoColaborador != null)
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;
                        else
                        {
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;
                        }

                        objetivoColaboradorDAO.Editar(objetivo);
                    }
                }

                if (model.OutrasContribuicoesCadastro != null)
                {
                    ContribuicaoColaborador contribuicao = new ContribuicaoColaborador();

                    contribuicao.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                    contribuicao.Contribuicao = model.OutrasContribuicoesCadastro.Contribuicao;

                    contribuicaoColaboradorDAO.Incluir(contribuicao);
                }
                else if (model.ListaOutrasContribuicoesViewModel != null)
                {
                    foreach (var item in model.ListaOutrasContribuicoesViewModel)
                    {
                        var contribuicao = contribuicaoColaboradorDAO.Obter(item.ID);

                        contribuicao.Contribuicao = item.Contribuicao;

                        contribuicaoColaboradorDAO.Editar(contribuicao);
                    }
                }
            }

            return ManterAvaliacaoColaboradorAutoAvaliacao(model.CicloAvaliacaoSelecionadoID);
        }

        [Authorize]
        public ActionResult DeletarAvaliacaoColaboradorAutoAvaliacao(int? objetivoSelecionadoID,
                                                                     int? cicloAvaliacaoSelecionadoID)
        {
            if (ModelState.IsValid)
            {
                if (objetivoSelecionadoID.HasValue)
                {
                    ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();

                    objetivoColaboradorDAO.Excluir(objetivoSelecionadoID.Value);
                }
            }

            return ManterAvaliacaoColaboradorAutoAvaliacao(cicloAvaliacaoSelecionadoID);
        }

        [Authorize]
        public ActionResult DeletarAvaliacaoColaboradorOutrasContribuicoes(int? contribuicaoSelecionadaID,
                                                                           int? cicloAvaliacaoSelecionadoID)
        {
            if (ModelState.IsValid)
            {
                if (contribuicaoSelecionadaID.HasValue)
                {
                    ContribuicaoColaboradorDAO contribuicaoColaboradorDAO = new ContribuicaoColaboradorDAO();

                    contribuicaoColaboradorDAO.Excluir(contribuicaoSelecionadaID.Value);
                }
            }

            return ManterAvaliacaoColaboradorAutoAvaliacao(cicloAvaliacaoSelecionadoID);
        }

        #endregion Auto-avaliação

        #region Competências

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorCompetencias(int? id,
                                                                   int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorCompetenciasViewModel model =
                new ManterAvaliacaoColaboradorCompetenciasViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

            if (avaliacaoColaborador != null)
            {
                model.GestorRubiID = avaliacaoColaborador.GestorRubiID;

                var associacaoCargoCompentencia = new AssociacaoCargoCompetenciaDAO().Obter(id.Value, identidade.CargoRubiID.Value, identidade.AreaRubiID.Value, identidade.SetorRubiID.Value);

                var competenciasCorporativas = new Integracoes.SistemaCompetencias.IntegracaoSistemaCompetencias().ListarCompentenciasCargo(associacaoCargoCompentencia.CargoCompetenciaID.Value, associacaoCargoCompentencia.AreaCompetenciaID.Value, associacaoCargoCompentencia.SetorCompetenciaID.Value);

                if (competenciasCorporativas != null)
                {
                    model.ListaCompetenciasCorporativas = new List<ItemListaCompetenciasColaborador>();
                    model.ListaCompetenciasFuncionais = new List<ItemListaCompetenciasColaborador>();
                    model.ListaCompetenciasLideranca = new List<ItemListaCompetenciasColaborador>();

                    foreach (var item in competenciasCorporativas)
                    {
                        var competenciasColaborador = new CompetenciaColaboradorDAO().Obter(id.Value, item.id_comp);

                        switch (item.id_tipo_comp)
                        {
                            case (int)Enumeradores.TipoCompetencia.Corporativa:
                                {
                                    model.ListaCompetenciasCorporativas.Add(new ItemListaCompetenciasColaborador
                                    {
                                        ID = (competenciasColaborador == null) ? 0 : competenciasColaborador.ID,
                                        NivelColaborador = (competenciasColaborador == null) ? 0 : ((competenciasColaborador.NivelColaborador.HasValue) ? competenciasColaborador.NivelColaborador.Value : 0),
                                        Competencia = item.descricao_comp,
                                        CompentenciaID = item.id_comp
                                    });
                                    break;
                                }
                            case (int)Enumeradores.TipoCompetencia.Funcionais:
                                {
                                    model.ListaCompetenciasFuncionais.Add(new ItemListaCompetenciasColaborador
                                    {
                                        ID = (competenciasColaborador == null) ? 0 : competenciasColaborador.ID,
                                        NivelColaborador = (competenciasColaborador == null) ? 0 : ((competenciasColaborador.NivelColaborador.HasValue) ? competenciasColaborador.NivelColaborador.Value : 0),
                                        Competencia = item.descricao_comp,
                                        CompentenciaID = item.id_comp
                                    });
                                    break;
                                }
                            case (int)Enumeradores.TipoCompetencia.Lideranca:
                                {
                                    model.ListaCompetenciasLideranca.Add(new ItemListaCompetenciasColaborador
                                    {
                                        ID = (competenciasColaborador == null) ? 0 : competenciasColaborador.ID,
                                        NivelColaborador = (competenciasColaborador == null) ? 0 : ((competenciasColaborador.NivelColaborador.HasValue) ? competenciasColaborador.NivelColaborador.Value : 0),
                                        Competencia = item.descricao_comp,
                                        CompentenciaID = item.id_comp
                                    });
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }

                var listaAval = new List<SelectListItem>();

                for (int i = 0; i < 5; i++)
                {
                    listaAval.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                
                model.ListaNivelAvaliacao = listaAval;

                model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;
            }
            else
                model.GestorRubiID = identidade.GestorRubiID;

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorCompetencias.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorCompetencias(ManterAvaliacaoColaboradorCompetenciasViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identidade = new Identidade();

                int usuarioID = identidade.UsuarioID;

                model.UsuarioRubiID = identidade.UsuarioRubiID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubiID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasCorporativas != null)
                    {
                        foreach (var item in model.ListaCompetenciasCorporativas)
                        {
                            competenciasCorporativas.Add(new CompetenciaColaborador
                            {
                                ID = item.ID.Value,
                                CompetenciaID = item.CompentenciaID,
                                AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                NivelColaborador = item.NivelColaborador
                            });
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }

                    competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasFuncionais != null)
                    {
                        foreach (var item in model.ListaCompetenciasFuncionais)
                        {
                            competenciasCorporativas.Add(new CompetenciaColaborador
                            {
                                ID = item.ID.Value,
                                CompetenciaID = item.CompentenciaID,
                                AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                NivelColaborador = item.NivelColaborador
                            });
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }

                    competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasLideranca != null)
                    {
                        foreach (var item in model.ListaCompetenciasLideranca)
                        {
                            competenciasCorporativas.Add(new CompetenciaColaborador
                            {
                                ID = item.ID.Value,
                                CompetenciaID = item.CompentenciaID,
                                AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                NivelColaborador = item.NivelColaborador
                            });
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;

                model.CicloAvaliacaoSelecionadoID = model.CicloAvaliacaoSelecionadoID.Value;
            }

            return ManterAvaliacaoColaboradorCompetencias(model.AvaliacaoColaboradorID, null);
        }

        public ActionResult ManterAvaliacaoColaboradorPerformance(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        {
            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorPerformance.cshtml", model);
        }

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ManterAvaliacaoColaboradorAutoAvaliacao(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();
        //        ContribuicaoColaboradorDAO contribuicaoColaboradorDAO = new ContribuicaoColaboradorDAO();

        //        if (model.ObjetivoMetaResultadoAtingidoCadastro != null)
        //        {
        //            ObjetivoColaborador objetivo = new ObjetivoColaborador();

        //            objetivo.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
        //            objetivo.Objetivo = model.ObjetivoMetaResultadoAtingidoCadastro.Objetivo;
        //            objetivo.AutoAvaliacao = true;

        //            objetivo.MetaColaborador = new MetaColaborador();
        //            objetivo.MetaColaborador.Meta = model.ObjetivoMetaResultadoAtingidoCadastro.MetaColaboradorMeta;

        //            objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
        //            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido
        //                = model.ObjetivoMetaResultadoAtingidoCadastro.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;

        //            objetivoColaboradorDAO.Incluir(objetivo);
        //        }
        //        else if (model.ListaObjetivosMetasResultadosatingidosViewModel != null)
        //        {
        //            foreach (var item in model.ListaObjetivosMetasResultadosatingidosViewModel)
        //            {
        //                var objetivo = objetivoColaboradorDAO.Obter(item.ID);

        //                objetivo.Objetivo = item.Objetivo;
        //                objetivo.MetaColaborador.Meta = item.MetaColaboradorMeta;

        //                if (objetivo.MetaColaborador.ResultadoAtingidoColaborador != null)
        //                    objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;
        //                else
        //                {
        //                    objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
        //                    objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;
        //                }

        //                objetivoColaboradorDAO.Editar(objetivo);
        //            }
        //        }

        //        if (model.OutrasContribuicoesCadastro != null)
        //        {
        //            ContribuicaoColaborador contribuicao = new ContribuicaoColaborador();

        //            contribuicao.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
        //            contribuicao.Contribuicao = model.OutrasContribuicoesCadastro.Contribuicao;

        //            contribuicaoColaboradorDAO.Incluir(contribuicao);
        //        }
        //        else if (model.ListaOutrasContribuicoesViewModel != null)
        //        {
        //            foreach (var item in model.ListaOutrasContribuicoesViewModel)
        //            {
        //                var contribuicao = contribuicaoColaboradorDAO.Obter(item.ID);

        //                contribuicao.Contribuicao = item.Contribuicao;

        //                contribuicaoColaboradorDAO.Editar(contribuicao);
        //            }
        //        }
        //    }

        //    return ManterAvaliacaoColaboradorAutoAvaliacao(model.CicloAvaliacaoSelecionadoID);
        //}

        #endregion Competências

        #region Manutenção PDI colaborador

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeDesenvolvimentoCompetenciaParaItemListaDesenvolvimentoCompetenciaViewModel))]
        public ActionResult ManterAvaliacaoPDIColaborador(int? id,
                                                          bool incluirDesenvolvimentoCompetencia = false,
                                                          int? colaboradorID = null)
        {
            ManterAvaliacaoPDIColaboradorViewModel model =
                new ManterAvaliacaoPDIColaboradorViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
            {
                usuarioID = colaboradorID.Value;
                model.Aprovar = true;
            }
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            var avaliacaoPDIColaborador =
                new AvaliacaoPDIColaboradorDAO().Obter(id.Value, usuarioID);

            if (avaliacaoPDIColaborador != null)
            {
                model.GestorRubiID = avaliacaoPDIColaborador.GestorRubiID;

                model.ListaDesenvolvimentoCompetenciaViewModel =
                    Mapper.Map<List<DesenvolvimentoCompetencia>,
                               List<ItemListaDesenvolvimentoCompetenciaViewModel>>
                                    (new DesenvolvimentoCompetenciaDAO().Listar(avaliacaoPDIColaborador.ID));

                model.AvaliacaoPDIColaboradorID = avaliacaoPDIColaborador.ID;

                model.StatusPDIID = avaliacaoPDIColaborador.StatusPDI_ID;

                model.Aprovada = avaliacaoPDIColaborador.Aprovada;

                model.Idiomas = avaliacaoPDIColaborador.Idiomas;

                model.Graduacao = avaliacaoPDIColaborador.Graduacao;

                model.PontosFortes = avaliacaoPDIColaborador.PontosFortes;

                model.PontosDesenvolvimento = avaliacaoPDIColaborador.PontosDesenvolvimento;

                model.ComentariosColaborador = avaliacaoPDIColaborador.ComentariosColaborador;

                model.ComentariosGestor = avaliacaoPDIColaborador.ComentariosGestor;
            }
            else
                model.GestorRubiID = identidade.GestorRubiID;

            model.CicloAvaliacaoSelecionadoID = id.Value;
            model.IncluirDesenvolvimentoCompetencia = incluirDesenvolvimentoCompetencia;

            return View("~/Views/Avaliacoes/ManterAvaliacaoPDIColaborador.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaViewModel))]
        public ActionResult ManterAvaliacaoPDIColaborador(ManterAvaliacaoPDIColaboradorViewModel model)
        {
            if (ModelState.IsValid)
            {
                AvaliacaoPDIColaboradorDAO avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();

                if (!model.AvaliacaoPDIColaboradorID.HasValue)
                {
                    var avaliacaoPDI = new AvaliacaoPDIColaborador();

                    var identidade = new Identidade();

                    avaliacaoPDI.DataCriacao = DateTime.Today;
                    avaliacaoPDI.CicloAvaliacao_ID = model.CicloAvaliacaoSelecionadoID.Value;
                    avaliacaoPDI.Colaborador_ID = identidade.UsuarioID;
                    avaliacaoPDI.GestorRubiID = identidade.GestorRubiID;
                    avaliacaoPDI.StatusPDI_ID = (int)Enumeradores.StatusPDI.Criada;
                    avaliacaoPDI.Idiomas = model.Idiomas;
                    avaliacaoPDI.Graduacao = model.Graduacao;
                    avaliacaoPDI.PontosFortes = model.PontosFortes;
                    avaliacaoPDI.PontosDesenvolvimento = model.PontosDesenvolvimento;
                    avaliacaoPDI.ComentariosColaborador = model.ComentariosColaborador;
                    
                    avaliacaoPDIColaboradorDAO.Incluir(avaliacaoPDI);

                    model.AvaliacaoPDIColaboradorID = avaliacaoPDI.ID;
                }
                else
                {
                    var avaliacaoPDI = avaliacaoPDIColaboradorDAO.Obter(model.CicloAvaliacaoSelecionadoID.Value, new Identidade().UsuarioID);

                    if(avaliacaoPDI != null)
                    {
                        avaliacaoPDI.Idiomas = model.Idiomas;
                        avaliacaoPDI.Graduacao = model.Graduacao;
                        avaliacaoPDI.PontosFortes = model.PontosFortes;
                        avaliacaoPDI.PontosDesenvolvimento = model.PontosDesenvolvimento;
                        avaliacaoPDI.ComentariosColaborador = model.ComentariosColaborador;

                        avaliacaoPDIColaboradorDAO.Editar(avaliacaoPDI);
                    }
                }

                DesenvolvimentoCompetenciaDAO desenvolvimentoCompetenciaDAO = new DesenvolvimentoCompetenciaDAO();

                if (model.DesenvolvimentoCompetenciaCadastro != null)
                {
                    DesenvolvimentoCompetencia desenvolvimentoCompetencia = new DesenvolvimentoCompetencia();

                    desenvolvimentoCompetencia.AvaliacaoPDIColaborador_ID = model.AvaliacaoPDIColaboradorID.Value;
                    desenvolvimentoCompetencia.AcaoDesenvolvimento = model.DesenvolvimentoCompetenciaCadastro.AcaoDesenvolvimento;
                    desenvolvimentoCompetencia.RescursoSuporte = model.DesenvolvimentoCompetenciaCadastro.RecursoSuporte;

                    desenvolvimentoCompetenciaDAO.Incluir(desenvolvimentoCompetencia);
                }
                else
                {
                    if (model.ListaDesenvolvimentoCompetenciaViewModel != null)
                    {
                        foreach (var item in model.ListaDesenvolvimentoCompetenciaViewModel)
                        {
                            var desenvolvimentoCompetencia = desenvolvimentoCompetenciaDAO.Obter(item.ID);

                            desenvolvimentoCompetencia.AcaoDesenvolvimento = item.AcaoDesenvolvimento;
                            desenvolvimentoCompetencia.RescursoSuporte = item.RecursoSuporte;

                            desenvolvimentoCompetenciaDAO.Editar(desenvolvimentoCompetencia);
                        }
                    }
                }
            }

            return ManterAvaliacaoPDIColaborador(model.CicloAvaliacaoSelecionadoID);
        }

        #endregion
    }
}