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

            model.CicloAvaliacaoDescricao = new CicloAvaliacaoDAO().Obter(id.Value).Descricao;

            return CarregarAvaliacoesColaboradoresPorCiclo(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarAvaliacoes(GestaoAvaliacoesColaboradoresViewModel model)
        {
            return CarregarAvaliacoesColaboradoresPorCiclo(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarPDIs(GestaoPDIsViewModel model)
        {
            return CarregarAvaliacoesPdiColaboradoresPorCiclo(model);
        }

        private ActionResult CarregarAvaliacoesColaboradoresPorCiclo(GestaoAvaliacoesColaboradoresViewModel model)
        {
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloAvaliacaoSelecionadoID);
            
            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);
            var gestoresCicloAvaliacao = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);

            if (avaliacoes != null
               && associacoesCargosCompetencias != null
                && gestoresCicloAvaliacao != null)
            {
                model.AvaliacoesColaboradores =
                    (from avaliacao in avaliacoes
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
                        //&& (avaliacao.Usuario != null) ? avaliacao.Usuario.Nome.ToUpper()
                        //                .Contains(!string.IsNullOrEmpty(model.GestorPesquisado)
                        //                          && !string.IsNullOrWhiteSpace(model.GestorPesquisado)
                        //                                ? model.GestorPesquisado.Trim().ToUpper()
                        //                                : avaliacao.Usuario.Nome.ToUpper()) : avaliacao == null
                        && avaliacao.StatusAvaliacaoColaborador_ID
                                        .Equals(model.StatusAvaliacaoPesquisadoID.HasValue
                                                        ? model.StatusAvaliacaoPesquisadoID.Value
                                                        : avaliacao.StatusAvaliacaoColaborador_ID)
                     select new ItemListaGestaoAvaliacoesViewModel()
                     {
                         ID = avaliacao.ID,
                         NomeGestor = (avaliacao.Usuario == null) ? "" : (gestoresCicloAvaliacao.Exists(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID) ? gestoresCicloAvaliacao.Find(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID).Nome : ""),
                         UsuarioNome = avaliacao.Usuario.Nome,
                         Cargo = associacao.CargoRubi,
                         Area = associacao.AreaRubi,
                         StatusAvaliacaoColaboradorNome = avaliacao.StatusAvaliacaoColaborador.Nome,
                         StatusAvaliacaoColaboradorID = avaliacao.StatusAvaliacaoColaborador_ID,
                         ColaboradorID = avaliacao.Usuario.ID
                     }).ToList();

                model.AvaliacoesColaboradores = model.AvaliacoesColaboradores
                                                     .Where(x => x.NomeGestor.Contains(!string.IsNullOrEmpty(model.GestorPesquisado)
                                                                                       && !string.IsNullOrWhiteSpace(model.GestorPesquisado)
                                                                                       ? model.GestorPesquisado.Trim().ToUpper()
                                                                                       : x.NomeGestor.ToUpper())).ToList();


                model.StatusAprovacaoAvaliacaoColaborador
                    = Mapper.Map<List<StatusAvaliacaoColaborador>,
                                 List<SelectListItem>>(new StatusAvaliacaoColaboradorDAO().ListarIN(2, 3));

                model.StatusObjetivoDefinidoAvaliacaoColaborador
                    = Mapper.Map<List<StatusAvaliacaoColaborador>,
                                 List<SelectListItem>>(new StatusAvaliacaoColaboradorDAO().ListarIN(2, 4));

                model.StatusEmAvaliacaoGestorAvaliacaoColaborador
                    = Mapper.Map<List<StatusAvaliacaoColaborador>,
                                 List<SelectListItem>>(new StatusAvaliacaoColaboradorDAO().ListarIN(5, 6));
            }

            model.StatusAvaliacaoColaborador
                    = Mapper.Map<List<StatusAvaliacaoColaborador>,
                                 List<SelectListItem>>(new StatusAvaliacaoColaboradorDAO().Listar());

            return View("~/Views/Avaliacoes/GestaoAvaliacoesColaboradores.cshtml", model);
        }

        private ActionResult CarregarAvaliacoesPdiColaboradoresPorCiclo(GestaoPDIsViewModel model)
        {
            var avaliacoes = new AvaliacaoPDIColaboradorDAO().Listar(model.CicloAvaliacaoSelecionadoID);
            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);
            var gestoresCicloAvaliacao = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID);

            if(avaliacoes != null
                && associacoesCargosCompetencias != null)
            {
                model.AvaliacoesPDIsColaboradores = 
                (from avaliacao in avaliacoes
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
                    && avaliacao.StatusPDI_ID
                                    .Equals(model.StatusPDIPesquisadoID.HasValue
                                                    ? model.StatusPDIPesquisadoID.Value
                                                    : avaliacao.StatusPDI_ID)
                 select new ItemListaGestaoAvaliacoesPDIViewModel()
                 {
                     ID = avaliacao.ID,
                     NomeGestor = (avaliacao.Usuario == null) ? "" : (gestoresCicloAvaliacao.Exists(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID) ? gestoresCicloAvaliacao.Find(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID).Nome : ""),
                     UsuarioNome = avaliacao.Usuario.Nome,
                     Cargo = associacao.CargoRubi,
                     Area = associacao.AreaRubi,
                     StatusPDINome = avaliacao.StatusPDI.Nome,
                     StatusPDIID = avaliacao.StatusPDI_ID,
                     ColaboradorID = avaliacao.Usuario.ID
                 }).ToList();

                model.AvaliacoesPDIsColaboradores = model.AvaliacoesPDIsColaboradores
                                                     .Where(x => x.NomeGestor.Contains(!string.IsNullOrEmpty(model.GestorPesquisado)
                                                                                       && !string.IsNullOrWhiteSpace(model.GestorPesquisado)
                                                                                       ? model.GestorPesquisado.Trim().ToUpper()
                                                                                       : x.NomeGestor.ToUpper())).ToList();
            }

            model.StatusPDI
                = Mapper.Map<List<StatusPDI>,
                             List<SelectListItem>>(new StatusPDIDAO().Listar());

            return View("~/Views/Avaliacoes/GestaoPDIs.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AlterarStatusAvaliacoes(GestaoAvaliacoesColaboradoresViewModel model)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacoesCiclo = avaliacaoColaboradorDAO.Listar(model.CicloAvaliacaoSelecionadoID);

            if (avaliacoesCiclo != null)
            {
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
                     GestorRubi_ID = avaliacao.GestorRubi_ID,
                     GestorRubiEmp_ID = avaliacao.GestorRubiEmp_ID,
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
            }

            return GestaoAvaliacoesColaboradores(model.CicloAvaliacaoSelecionadoID);
        }

        #endregion Gestão avaliações colaboradores de um ciclo

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeStatusPDIParaSelectListItem))]
        public ActionResult GestaoPDIs(int? id)
        {
            GestaoPDIsViewModel model = new GestaoPDIsViewModel();

            model.CicloAvaliacaoSelecionadoID = id.Value;

            model.CicloAvaliacaoDescricao = new CicloAvaliacaoDAO().Obter(id.Value).Descricao;

            return CarregarAvaliacoesPdiColaboradoresPorCiclo(model);
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
            if (ModelState.IsValid)
            {
                return CarregarGestaoAvaliacaoColaborador(modelSelecao.CicloAvaliacaoSelecionadoID, identidade);
            }

            return RedirectToAction("Index", "Home");
        }

        private ActionResult CarregarGestaoAvaliacaoColaborador(int? cicloAvaliacaoSelecionadoID,
                                                                Identidade identidade)
        {
            GestaoAvaliacaoColaboradorViewModel model = new GestaoAvaliacaoColaboradorViewModel();

            var avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();
            var avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();
            var associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(cicloAvaliacaoSelecionadoID.Value);

            if (cicloAvaliacao != null)
            {
                model.OrientacaoCompetencia = cicloAvaliacao.OrientacaoCompetencia;
                model.URLCompetencia = cicloAvaliacao.URLCompetencia;
                model.SituacaoCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;
                model.TituloOrientacaoObjetivosMetas = cicloAvaliacao.TituloOrientacaoObjetivosMetas;
                model.TextoOrientacaoObjetivosMetas = cicloAvaliacao.TextoOrientacaoObjetivosMetas;
                model.TituloOrientacaoAutoAvaliacao = cicloAvaliacao.TituloOrientacaoAutoAvaliacao;
                model.TextoOrientacaoAutoAvaliacao = cicloAvaliacao.TextoOrientacaoAutoAvaliacao;
                model.TituloOrientacaoAvaliacaoGestor = cicloAvaliacao.TituloOrientacaoAvaliacaoGestor;
                model.TextoOrientacaoAvaliacaoGestor = cicloAvaliacao.TextoOrientacaoAvaliacaoGestor;
            }


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
                                                                                identidade.UsuarioRubiID.Value,
                                                                                identidade.CodigoEmpresaRubiUD);

            var associacoesCargosCompetencias = associacaoCargoCompetenciaDAO.ListarPorCicloAvaliacao(cicloAvaliacaoSelecionadoID.Value);

            if (associacoesCargosCompetencias != null)
            {
                var existeCargo = associacoesCargosCompetencias.Where(x => x.AreaRubiID == identidade.AreaRubiID &&
                                                                           x.SetorRubiID == identidade.SetorRubiID &&
                                                                           x.CargoRubiID == identidade.CargoRubiID &&
                                                                           x.AreaCompetenciaID != null &&
                                                                           x.CargoCompetenciaID != null &&
                                                                           x.SetorCompetenciaID != null);
                model.ParticipaDaAvaliacao = (existeCargo.Any());
            }

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

            var pdiCicloGestor = new AvaliacaoPDIColaboradorDAO().ListarPorGestor(cicloAvaliacaoSelecionadoID.Value,
                                                                                identidade.UsuarioRubiID.Value,
                                                                                identidade.CodigoEmpresaRubiUD);

            if (pdiCicloGestor != null
                && associacoesCargosCompetencias != null)
            {
                model.ListaGestaoAvaliacaoPDIColaboradorViewModel =
                    (from avaliacao in avaliacoesCicloGestor
                     join pdi in pdiCicloGestor
                           on new { avaliacao.CicloAvaliacao_ID, avaliacao.Colaborador_ID } equals new { pdi.CicloAvaliacao_ID, pdi.Colaborador_ID }
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
                     select new ItemListaGestaoAvaliacaoPDIColaboradorViewModel()
                     {
                         DataCriacao = pdi.DataCriacao.ToShortDateString(),
                         UsuarioID = pdi.Colaborador_ID,
                         UsuarioNome = pdi.Usuario.Nome,
                         Cargo = associacao.CargoRubi,
                         StatusAvaliacaoPDIColaboradorNome = pdi.StatusPDI.Nome
                     }).ToList();
            }

            model.CicloAvaliacaoSelecionadoID = cicloAvaliacaoSelecionadoID;

            model.ExibirAutoAvaliacao = this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("ExibirAutoAvaliao" + model.CicloAvaliacaoSelecionadoID.Value);

            model.ExibirAvaliacaoGestor = this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("ExibirAvaliacaoGestor" + model.CicloAvaliacaoSelecionadoID.Value);

            model.ExibirObjetivosMetas = this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("ExibirObjetivosMetas" + model.CicloAvaliacaoSelecionadoID.Value);

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

            var identidade = new Identidade();

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
            {
                usuarioID = identidade.UsuarioID;
                colaboradorID = usuarioID;
            }

            var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

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
                            {
                                if (identidade.UsuarioID == usuarioID)
                                    return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(id, colaboradorID);
                            }

                        case ((int)Enumeradores.EtapasAutoAvaliacao.Competencias):
                            return ManterAvaliacaoColaboradorCompetencias(id, colaboradorID);
                        case ((int)Enumeradores.EtapasAutoAvaliacao.Performance):
                            return ManterAvaliacaoDesempenho(id, false, colaboradorID);

                        default:
                            {
                                if (identidade.UsuarioID == usuarioID)
                                    return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(id, colaboradorID);
                            }
                    }
                }
                else
                {
                    if (identidade.UsuarioID == usuarioID)
                        return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                    return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(id, colaboradorID);
                }
            }
            else if (avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                        .Equals((int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosGestores))
            {
                if (etapaAutoAvaliacao.HasValue)
                {
                    switch (etapaAutoAvaliacao.Value)
                    {
                        case ((int)Enumeradores.EtapasAutoAvaliacao.Competencias):
                            return ManterAvaliacaoColaboradorCompetencias(id, colaboradorID);
                        default:
                            {
                                if (identidade.UsuarioID == usuarioID)
                                    return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(id, colaboradorID);
                            }
                    }
                    
                }
            }
            else if (avaliacaoColaborador.StatusAvaliacaoColaborador_ID
                       .Equals((int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH) ||
                    avaliacaoColaborador.StatusAvaliacaoColaborador_ID  
                       .Equals((int)Enumeradores.StatusAvaliacaoColaborador.Encerrada))
            {
                //if (identidade.UsuarioID == usuarioID)
                //    return ManterAvaliacaoColaboradorAutoAvaliacao(id, false, false, colaboradorID);
                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(id, colaboradorID);
            }
            return RedirectToAction("Index", "Home");
        }

        #region Desempenho e recomendações

        [Authorize]
    [HttpGet]
    [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaViewModel))]
    public ActionResult ManterAvaliacaoDesempenho(int? id,
                                                                    bool incluirMeta = false,
                                                                    int? colaboradorID = null)
    {
        ManterAvaliacaoColaboradorCompetenciasViewModel model =
            new ManterAvaliacaoColaboradorCompetenciasViewModel();

        var identidade = new Identidade();

        int usuarioID;

        if (colaboradorID.HasValue)
        {
            usuarioID = colaboradorID.Value;
            //model.Aprovar = true;
        }
        else
            usuarioID = identidade.UsuarioID;

        model.UsuarioRubiID = identidade.UsuarioRubiID;

        #region <<<<< Validar permissão de usuário >>>>>
        if (identidade.UsuarioID != usuarioID)
        {
            if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
            {
                if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                {
                    return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                }
            }
        }
        #endregion

        var avaliacaoColaborador =
            new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

        if (avaliacaoColaborador != null)
        {
            model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
            model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

            //model.ListaObjetivosMetasViewModel =
            //    Mapper.Map<List<ObjetivoColaborador>,
            //                List<ObjetivoMetaViewModel>>
            //                    (new ObjetivoColaboradorDAO().Listar(avaliacaoColaborador.ID));

            model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

            model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

            //model.Aprovada = avaliacaoColaborador.Aprovada;

            //model.JustificativaReprovacao = avaliacaoColaborador.JustificativaReprovacao;
        }
        else
            model.GestorRubiID = identidade.GestorRubiID;

        model.CicloAvaliacaoSelecionadoID = id.Value;
       // model.IncluirMeta = incluirMeta;

        return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorCompetencias.cshtml", model);
    }

        #endregion
 
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

            model.ColaboradorID = usuarioID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioObjetivosMetas = cicloAvaliacao.DataInicioObjetivosMetas;
                model.DataTerminoObjetivosMetas = cicloAvaliacao.DataTerminoObjetivosMetas;
                model.SituacaoCicloAvaliaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

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
            }

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

                    var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value);

                    if (ciclo != null)
                    {
                        var identidade = new Identidade();

                        avaliacao.DataCriacao = DateTime.Today;
                        avaliacao.CicloAvaliacao_ID = model.CicloAvaliacaoSelecionadoID.Value;
                        avaliacao.Colaborador_ID = identidade.UsuarioID;
                        avaliacao.GestorRubi_ID = identidade.GestorRubiID;
                        avaliacao.GestorRubiEmp_ID = identidade.CodigoEmpresaRubiUD;
                        if (ciclo.SituacaoCicloAvaliacao_ID.Value == 1 || ciclo.SituacaoCicloAvaliacao_ID.Value == 2)
                        {
                            avaliacao.StatusAvaliacaoColaborador_ID = ciclo.SituacaoCicloAvaliacao_ID.Value;
                        }
                        else if (ciclo.SituacaoCicloAvaliacao_ID.Value == 3)
                        {
                            avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.ObjetivosMetasDefinidos;
                        }
                        else if (ciclo.SituacaoCicloAvaliacao_ID.Value == 4)
                        {
                            avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.AutoAvaliacao;
                        }
                        else
                        {
                            avaliacao.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.DefinicaoObjetivosMetas;
                        }
                        avaliacao.CargoRubiID = identidade.CargoRubiID;
                        avaliacao.AreaRubiID = identidade.AreaRubiID;
                        avaliacao.SetorRubiID = identidade.SetorRubiID;

                        avaliacaoColaboradorDAO.Incluir(avaliacao);

                        model.AvaliacaoColaboradorID = avaliacao.ID;
                    }
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

                return ManterAvaliacaoColaboradorObjetivosMetas(model.CicloAvaliacaoSelecionadoID);
            }
            else
            {
                return View(model);
            }
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
        public ActionResult SubmeterAvaliacaoColaboradorAvaliacaoRH(int? cicloAvaliacaoSelecionadoID,
                                                                        int? avaliacaoColaboradorID,
                                                                        [ModelBinder(typeof(IdentidadeModelBinder))] Identidade identidade)
        {
            AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

            var avaliacaoColaborador = avaliacaoColaboradorDAO.Obter(avaliacaoColaboradorID.Value);

            //Se tiver objetivos sem avaliacao, não deixar submeter ao RH.
            if (new ObjetivoColaboradorDAO().ExisteObjetivoSemAvaliacaoGestor(avaliacaoColaboradorID.Value))
            {
                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(cicloAvaliacaoSelecionadoID, avaliacaoColaborador.Colaborador_ID);
            }

            //Se tiver competencias sem avaliacao do gestor, não deixar submeter ao RH.
            if (new CompetenciaColaboradorDAO().ExisteCompetenciaSemAvaliacaoGestor(avaliacaoColaboradorID.Value))
            {
                return ManterAvaliacaoColaboradorCompetenciasGestor(cicloAvaliacaoSelecionadoID, avaliacaoColaborador.Colaborador_ID);
            }

            //Se não tiver perfomance, não deixar submeter ao RH.
            var performance = new PerformanceColaboradorDAO().Obter(avaliacaoColaboradorID.Value);
            if (performance == null)
            {
                return ManterAvaliacaoColaboradorPerformance(cicloAvaliacaoSelecionadoID, avaliacaoColaborador.Colaborador_ID);
            }
            else
            {
                if (string.IsNullOrEmpty(performance.AvaliacaoPerformance))
                {
                    return ManterAvaliacaoColaboradorPerformance(cicloAvaliacaoSelecionadoID, avaliacaoColaborador.Colaborador_ID);
                }
            }

            avaliacaoColaborador.StatusAvaliacaoColaborador_ID = (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH;

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
            if (model.JustificativaReprovacao == null)
            {
                ModelState.AddModelError("JustificativaReprovacao", "A justificatva da reprovação é obrigatória");
            }
            if (ModelState.IsValid)
            {
                return AprovarReprovarAvaliacaoColaborador(model.CicloAvaliacaoSelecionadoID, model.AvaliacaoColaboradorID, false, model.JustificativaReprovacao);
            }
            else
            {
                return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorObjetivosMetas.cshtml", model);
            }
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

            model.ColaboradorID = colaboradorID;

            model.ProximaEtapa = true;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAutoAvaliacao = cicloAvaliacao.DataInicioAutoAvaliacao;
                model.DataTerminoAutoAvaliacao = cicloAvaliacao.DataTerminoAutoAvaliacao;
                model.SituacaoCicloAvaliaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.ListaObjetivosMetasResultadosatingidosViewModel =
                        Mapper.Map<List<ObjetivoColaborador>,
                                   List<ObjetivoMetaResultadoAtingidoViewModel>>
                                        (new ObjetivoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                    model.ListaOutrasContribuicoesViewModel =
                        Mapper.Map<List<ContribuicaoColaborador>,
                                   List<OutrasContribuicoesViewModel>>
                                       (new ContribuicaoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                    model.ListaAvaliacaoGestorMetas = new List<AvaliacaoGestorContribuinte>();

                    var avaliacaoGestorMetas = (new AvaliacaoGestorDAO().ListarPorAvaliacaoID(avaliacaoColaborador.ID));

                    if (avaliacaoGestorMetas != null)
                    {
                        foreach (var item in avaliacaoGestorMetas)
                        {
                            model.ListaAvaliacaoGestorMetas.Add(new AvaliacaoGestorContribuinte
                            {
                                ID = item.ID,
                                Avaliacao = item.Avaliacao
                            });
                        }
                    }

                    model.ListaAvaliacaoGestorOutrasContribuicoes = new List<AvaliacaoGestorContribuinte>();

                    var avaliacaoGestorOutrasContribuicoes = (new AvaliacaoGestorDAO().ListarPorAvaliacaoID(avaliacaoColaborador.ID));

                    if (avaliacaoGestorOutrasContribuicoes != null)
                    {
                        foreach (var item in avaliacaoGestorOutrasContribuicoes)
                        {
                            model.ListaAvaliacaoGestorOutrasContribuicoes.Add(new AvaliacaoGestorContribuinte
                            {
                                ID = item.ID,
                                Avaliacao = item.Avaliacao
                            });
                        }
                    }

                    if (model.ListaAvaliacaoGestorMetas.Count == 0)
                    {
                        model.ListaAvaliacaoGestorMetas = new List<Models.Avaliacoes.AvaliacaoGestorContribuinte>();
                        foreach (var item in model.ListaObjetivosMetasResultadosatingidosViewModel)
                        {
                            model.ListaAvaliacaoGestorMetas.Add(new Models.Avaliacoes.AvaliacaoGestorContribuinte { ID = item.ID, Avaliacao = string.Empty });
                        }
                    }

                    if (model.ListaAvaliacaoGestorOutrasContribuicoes.Count == 0)
                    {
                        model.ListaAvaliacaoGestorOutrasContribuicoes = new List<Models.Avaliacoes.AvaliacaoGestorContribuinte>();
                        foreach (var item in model.ListaOutrasContribuicoesViewModel)
                        {
                            model.ListaAvaliacaoGestorOutrasContribuicoes.Add(new Models.Avaliacoes.AvaliacaoGestorContribuinte { ID = item.ID, Avaliacao = string.Empty });
                        }

                    }


                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;//avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;
            model.IncluirMeta = incluirMeta;
            model.IncluirContribuicao = incluirContribuicao;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorAutoAvaliacao.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManterAvaliacaoGestor(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        {
            return ManterAvaliacaoColaboradorAutoAvaliacao(model);
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
                        {
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;

                            if (objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor_ID.HasValue)
                            {
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor = new AvaliacaoGestorDAO().Obter(objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor_ID.Value);
                            }
                            else
                            {
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor = new AvaliacaoGestor();
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = "";
                            }

                            //if (item.AvaliacaoGestor == null)
                            //    objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = "";
                            //else
                            //    objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = item.AvaliacaoGestor;
                        }
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
                    if (model.AvaliacaoColaboradorID.HasValue)
                    {
                        contribuicao.AvaliacaoColaborador = new AvaliacaoColaboradorDAO().Obter(model.AvaliacaoColaboradorID.Value);
                    }

                    contribuicao.Contribuicao = model.OutrasContribuicoesCadastro.Contribuicao;

                    contribuicaoColaboradorDAO.Incluir(contribuicao);
                }
                else if (model.ListaOutrasContribuicoesViewModel != null)
                {
                    foreach (var item in model.ListaOutrasContribuicoesViewModel)
                    {
                        var contribuicao = contribuicaoColaboradorDAO.Obter(item.ID);

                        contribuicao.Contribuicao = item.Contribuicao;

                        if (contribuicao.AvaliacaoGestor_ID.HasValue)
                        {
                            contribuicao.AvaliacaoGestor = new AvaliacaoGestorDAO().Obter(contribuicao.AvaliacaoGestor_ID.Value);
                        }
                        else
                        {
                            contribuicao.AvaliacaoGestor = new AvaliacaoGestor();
                            contribuicao.AvaliacaoGestor.Avaliacao = "";
                        }
                        //contribuicao.AvaliacaoGestor.Avaliacao = item.AvaliacaoGestor;

                        contribuicaoColaboradorDAO.Editar(contribuicao);
                    }
                }

                return ManterAvaliacaoColaboradorAutoAvaliacao(model.CicloAvaliacaoSelecionadoID, model.IncluirMeta, false, model.ColaboradorID);
            }

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorAutoAvaliacao.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirMetaAvaliacaoColaboradorAutoAvaliacao(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        {
            model.IncluirMeta = true;
            model.IncluirContribuicao = false;
            return ManterAvaliacaoColaboradorAutoAvaliacao(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirContribuicaoAvaliacaoColaboradorAutoAvaliacao(ManterAvaliacaoColaboradorAutoAvaliacaoViewModel model)
        {
            model.IncluirMeta = false;
            model.IncluirContribuicao = true;
            return ManterAvaliacaoColaboradorAutoAvaliacao(model);
        }

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoGestorViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesGestorViewModel))]
        public ActionResult ManterAvaliacaoColaboradorAutoAvaliacaoGestor(int? id,
                                                                          int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorAutoAvaliacaoGestorViewModel model =
                new ManterAvaliacaoColaboradorAutoAvaliacaoGestorViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = colaboradorID;

            model.ProximaEtapa = true;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAvaliacaoGestor = cicloAvaliacao.DataInicioAvaliacaoGestor;
                model.DataTerminoAvaliacaoGestor = cicloAvaliacao.DataTerminoAvaliacaoGestor;
                model.SituacaoCicloAvaliaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;
                model.DataInicioVigencia = cicloAvaliacao.DataInicioVigencia;
                model.DataTerminoVigencia = cicloAvaliacao.DataFimVigencia;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    Mapper.CreateMap<ObjetivoColaborador, ObjetivoMetaResultadoAtingidoGestorViewModel>()
                    .ForMember(dest => dest.AvaliacaoGestor,
                               opt => opt.MapFrom(source => source.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao));

                    model.ListaObjetivosMetasResultadosatingidosGestorViewModel =
                        Mapper.Map<List<ObjetivoColaborador>,
                                   List<ObjetivoMetaResultadoAtingidoGestorViewModel>>
                                        (new ObjetivoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                    Mapper.CreateMap<ContribuicaoColaborador, OutrasContribuicoesGestorViewModel>()
                    .ForMember(dest => dest.AvaliacaoGestor,
                               opt => opt.MapFrom(source => source.AvaliacaoGestor.Avaliacao));

                    model.ListaOutrasContribuicoesGestorViewModel =
                        Mapper.Map<List<ContribuicaoColaborador>,
                                   List<OutrasContribuicoesGestorViewModel>>
                                       (new ContribuicaoColaboradorDAO().Listar(avaliacaoColaborador.ID));

                    model.ListaAvaliacaoGestorMetas = new List<AvaliacaoGestorContribuinte>();

                    var avaliacaoGestorMetas = (new AvaliacaoGestorDAO().ListarPorAvaliacaoID(avaliacaoColaborador.ID));

                    if (avaliacaoGestorMetas != null)
                    {
                        foreach (var item in avaliacaoGestorMetas)
                        {
                            model.ListaAvaliacaoGestorMetas.Add(new AvaliacaoGestorContribuinte
                            {
                                ID = item.ID,
                                Avaliacao = item.Avaliacao
                            });
                        }
                    }

                    model.ListaAvaliacaoGestorOutrasContribuicoes = new List<AvaliacaoGestorContribuinte>();

                    var avaliacaoGestorOutrasContribuicoes = (new AvaliacaoGestorDAO().ListarPorAvaliacaoID(avaliacaoColaborador.ID));

                    if (avaliacaoGestorOutrasContribuicoes != null)
                    {
                        foreach (var item in avaliacaoGestorOutrasContribuicoes)
                        {
                            model.ListaAvaliacaoGestorOutrasContribuicoes.Add(new AvaliacaoGestorContribuinte
                            {
                                ID = item.ID,
                                Avaliacao = item.Avaliacao
                            });
                        }
                    }

                    if (model.ListaAvaliacaoGestorMetas.Count == 0)
                    {
                        model.ListaAvaliacaoGestorMetas = new List<Models.Avaliacoes.AvaliacaoGestorContribuinte>();
                        foreach (var item in model.ListaObjetivosMetasResultadosatingidosGestorViewModel)
                        {
                            model.ListaAvaliacaoGestorMetas.Add(new Models.Avaliacoes.AvaliacaoGestorContribuinte { ID = item.ID, Avaliacao = string.Empty });
                        }
                    }

                    if (model.ListaAvaliacaoGestorOutrasContribuicoes.Count == 0)
                    {
                        model.ListaAvaliacaoGestorOutrasContribuicoes = new List<Models.Avaliacoes.AvaliacaoGestorContribuinte>();
                        foreach (var item in model.ListaOutrasContribuicoesGestorViewModel)
                        {
                            model.ListaAvaliacaoGestorOutrasContribuicoes.Add(new Models.Avaliacoes.AvaliacaoGestorContribuinte { ID = item.ID, Avaliacao = string.Empty });
                        }
                    }

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorAutoAvaliacaoGestor.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManterAvaliacaoColaboradorAutoAvaliacaoGestor(ManterAvaliacaoColaboradorAutoAvaliacaoGestorViewModel model)
        {
            if (ModelState.IsValid)
            {
                ObjetivoColaboradorDAO objetivoColaboradorDAO = new ObjetivoColaboradorDAO();
                ContribuicaoColaboradorDAO contribuicaoColaboradorDAO = new ContribuicaoColaboradorDAO();

                if (model.ObjetivoMetaResultadoAtingidoGestorCadastro != null)
                {
                    ObjetivoColaborador objetivo = new ObjetivoColaborador();

                    objetivo.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                    objetivo.Objetivo = model.ObjetivoMetaResultadoAtingidoGestorCadastro.Objetivo;
                    objetivo.AutoAvaliacao = true;

                    objetivo.MetaColaborador = new MetaColaborador();
                    objetivo.MetaColaborador.Meta = model.ObjetivoMetaResultadoAtingidoGestorCadastro.MetaColaboradorMeta;

                    objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
                    objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido
                        = model.ObjetivoMetaResultadoAtingidoGestorCadastro.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;

                    objetivoColaboradorDAO.Incluir(objetivo);
                }
                else if (model.ListaObjetivosMetasResultadosatingidosGestorViewModel != null)
                {
                    foreach (var item in model.ListaObjetivosMetasResultadosatingidosGestorViewModel)
                    {
                        var objetivo = objetivoColaboradorDAO.Obter(item.ID);

                        objetivo.Objetivo = item.Objetivo;
                        objetivo.MetaColaborador.Meta = item.MetaColaboradorMeta;

                        if (objetivo.MetaColaborador.ResultadoAtingidoColaborador != null)
                        {
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;

                            if (objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor_ID.HasValue)
                            {
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor = new AvaliacaoGestorDAO().Obter(objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor_ID.Value);
                            }
                            else
                            {
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor = new AvaliacaoGestor();
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = "";
                            }

                            if (item.AvaliacaoGestor == null)
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = "";
                            else
                                objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao = item.AvaliacaoGestor;
                        }
                        else
                        {
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador = new ResultadoAtingidoColaborador();
                            objetivo.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido = item.MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido;
                        }

                        objetivoColaboradorDAO.Editar(objetivo);
                    }
                }

                if (model.OutrasContribuicoesGestorCadastro != null)
                {
                    ContribuicaoColaborador contribuicao = new ContribuicaoColaborador();

                    contribuicao.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                    if (model.AvaliacaoColaboradorID.HasValue)
                    {
                        contribuicao.AvaliacaoColaborador = new AvaliacaoColaboradorDAO().Obter(model.AvaliacaoColaboradorID.Value);
                    }

                    contribuicao.Contribuicao = model.OutrasContribuicoesGestorCadastro.Contribuicao;

                    contribuicaoColaboradorDAO.Incluir(contribuicao);
                }
                else if (model.ListaOutrasContribuicoesGestorViewModel != null)
                {
                    foreach (var item in model.ListaOutrasContribuicoesGestorViewModel)
                    {
                        var contribuicao = contribuicaoColaboradorDAO.Obter(item.ID);

                        contribuicao.Contribuicao = item.Contribuicao;

                        if (contribuicao.AvaliacaoGestor_ID.HasValue)
                        {
                            contribuicao.AvaliacaoGestor = new AvaliacaoGestorDAO().Obter(contribuicao.AvaliacaoGestor_ID.Value);
                        }
                        else
                        {
                            contribuicao.AvaliacaoGestor = new AvaliacaoGestor();
                        }
                        if (item.AvaliacaoGestor == null)
                        {
                            contribuicao.AvaliacaoGestor.Avaliacao = "";
                        }
                        else
                        {
                            contribuicao.AvaliacaoGestor.Avaliacao = item.AvaliacaoGestor;
                        }
                        

                        contribuicaoColaboradorDAO.Editar(contribuicao);
                    }
                }

                return ManterAvaliacaoColaboradorAutoAvaliacaoGestor(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID);
            }

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorAutoAvaliacaoGestor.cshtml", model);
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

        [Authorize]
        public ActionResult DeletarDesenvolvimentoCompetenciaPDIColaborador(int? desenvolvimentoCompetenciaID,
                                                                           int? cicloAvaliacaoID)
        {
            if (ModelState.IsValid)
            {
                if (desenvolvimentoCompetenciaID.HasValue)
                {
                    DesenvolvimentoCompetenciaDAO desenvolvimentoCompetenciaDAO = new DesenvolvimentoCompetenciaDAO();

                    desenvolvimentoCompetenciaDAO.Excluir(desenvolvimentoCompetenciaID.Value);

                }
            }

            return ManterAvaliacaoPDIColaborador(cicloAvaliacaoID);
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
            {
                usuarioID = colaboradorID.Value;
                //identidade = new Identidade(usuarioID);
            }
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = usuarioID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAutoAvaliacao = cicloAvaliacao.DataInicioAutoAvaliacao;
                model.DataTerminoAutoAvaliacao = cicloAvaliacao.DataTerminoAutoAvaliacao;
                model.StatusCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    var associacaoCargoCompentencia = new AssociacaoCargoCompetenciaDAO().Obter(id.Value, avaliacaoColaborador.CargoRubiID.Value, avaliacaoColaborador.AreaRubiID.Value, avaliacaoColaborador.SetorRubiID.Value);

                    var competenciasCorporativas = new Integracoes.SistemaCompetencias.IntegracaoSistemaCompetencias().ListarCompentenciasCargo(associacaoCargoCompentencia.CargoCompetenciaID.Value, associacaoCargoCompentencia.AreaCompetenciaID.Value, associacaoCargoCompentencia.SetorCompetenciaID.Value);

                    if (competenciasCorporativas != null)
                    {
                        model.ListaCompetenciasCorporativas = new List<ItemListaCompetenciasColaborador>();
                        model.ListaCompetenciasFuncionais = new List<ItemListaCompetenciasColaborador>();
                        model.ListaCompetenciasLideranca = new List<ItemListaCompetenciasColaborador>();

                        foreach (var item in competenciasCorporativas)
                        {
                            var competenciasColaborador = new CompetenciaColaboradorDAO().Obter(avaliacaoColaborador.ID, item.id_comp);

                            ItemListaCompetenciasColaborador itemListaAdd = new ItemListaCompetenciasColaborador();

                            itemListaAdd.ID = (competenciasColaborador == null) ? 0 : competenciasColaborador.ID;
                            itemListaAdd.NivelColaborador = (competenciasColaborador == null) ? null : competenciasColaborador.NivelColaborador;
                            itemListaAdd.Competencia = (string.IsNullOrEmpty(item.descricao_comp)) ? item.titulo_comp :  item.titulo_comp + " - " + item.descricao_comp;
                            itemListaAdd.CompentenciaID = item.id_comp;

                            switch (item.id_tipo_comp)
                            {
                                case (int)Enumeradores.TipoCompetencia.Corporativa:
                                    {
                                        model.ListaCompetenciasCorporativas.Add(itemListaAdd);
                                        break;
                                    }
                                case (int)Enumeradores.TipoCompetencia.Funcionais:
                                    {
                                        model.ListaCompetenciasFuncionais.Add(itemListaAdd);
                                        break;
                                    }
                                case (int)Enumeradores.TipoCompetencia.Lideranca:
                                    {
                                        model.ListaCompetenciasLideranca.Add(itemListaAdd);
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
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            model.LiberadoPraSubmeterAoGestor = false;

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
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

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

                return ManterAvaliacaoColaboradorCompetencias(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID);
            }

            var listaAval = new List<SelectListItem>();

            for (int i = 0; i < 5; i++)
            {
                listaAval.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            model.ListaNivelAvaliacao = listaAval;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorCompetencias.cshtml", model);
        }

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorCompetenciasGestor(int? id,
                                                                         int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorCompetenciasGestorViewModel model =
                new ManterAvaliacaoColaboradorCompetenciasGestorViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
            {
                usuarioID = colaboradorID.Value;
                //identidade = new Identidade(usuarioID);
            }
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = usuarioID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAvaliacaoGestor = cicloAvaliacao.DataInicioAvaliacaoGestor;
                model.DataTerminoAvaliacaoGestor = cicloAvaliacao.DataTerminoAvaliacaoGestor;
                model.StatusCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    var associacaoCargoCompentencia = new AssociacaoCargoCompetenciaDAO().Obter(id.Value, avaliacaoColaborador.CargoRubiID.Value, avaliacaoColaborador.AreaRubiID.Value, avaliacaoColaborador.SetorRubiID.Value);

                    var competenciasCorporativas = new Integracoes.SistemaCompetencias.IntegracaoSistemaCompetencias().ListarCompentenciasCargo(associacaoCargoCompentencia.CargoCompetenciaID.Value, associacaoCargoCompentencia.AreaCompetenciaID.Value, associacaoCargoCompentencia.SetorCompetenciaID.Value);

                    if (competenciasCorporativas != null)
                    {
                        model.ListaCompetenciasCorporativas = new List<ItemListaCompetenciasColaboradorGestor>();
                        model.ListaCompetenciasFuncionais = new List<ItemListaCompetenciasColaboradorGestor>();
                        model.ListaCompetenciasLideranca = new List<ItemListaCompetenciasColaboradorGestor>();

                        foreach (var item in competenciasCorporativas)
                        {
                            var competenciasColaborador = new CompetenciaColaboradorDAO().Obter(avaliacaoColaborador.ID, item.id_comp);

                            ItemListaCompetenciasColaboradorGestor itemListaAdd = new ItemListaCompetenciasColaboradorGestor();

                            itemListaAdd.ID = (competenciasColaborador == null) ? 0 : competenciasColaborador.ID;
                            itemListaAdd.NivelColaborador = (competenciasColaborador == null) ? null : competenciasColaborador.NivelColaborador;
                            itemListaAdd.Competencia = (string.IsNullOrEmpty(item.descricao_comp)) ? item.titulo_comp : item.titulo_comp + " - " + item.descricao_comp;
                            itemListaAdd.CompentenciaID = item.id_comp;
                            int nivelRequerido;
                            if (Int32.TryParse(item.proeficiencia_cp, out nivelRequerido))
                            {
                                itemListaAdd.NivelRequerido = nivelRequerido;
                            }

                            itemListaAdd.NivelGestor = (competenciasColaborador == null) ? null : competenciasColaborador.NivelGestor;
                            itemListaAdd.ComentarioGestor = (competenciasColaborador == null) ? string.Empty : competenciasColaborador.ComentariosGestor;

                            switch (item.id_tipo_comp)
                            {
                                case (int)Enumeradores.TipoCompetencia.Corporativa:
                                    {
                                        model.ListaCompetenciasCorporativas.Add(itemListaAdd);
                                        break;
                                    }
                                case (int)Enumeradores.TipoCompetencia.Funcionais:
                                    {
                                        model.ListaCompetenciasFuncionais.Add(itemListaAdd);
                                        break;
                                    }
                                case (int)Enumeradores.TipoCompetencia.Lideranca:
                                    {
                                        model.ListaCompetenciasLideranca.Add(itemListaAdd);
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
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            model.ProximaEtapa = true;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorCompetenciasGestor.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorCompetenciasGestor(ManterAvaliacaoColaboradorCompetenciasGestorViewModel model)
        {
            if (model.ListaCompetenciasCorporativas != null)
            {
                for (int i = 0; i < model.ListaCompetenciasCorporativas.Count; i++)
                {
                    if (model.ListaCompetenciasCorporativas[i].NivelColaborador != model.ListaCompetenciasCorporativas[i].NivelGestor
                        && string.IsNullOrEmpty(model.ListaCompetenciasCorporativas[i].ComentarioGestor))
                    {
                        ModelState.AddModelError("ListaCompetenciasCorporativas[" + i.ToString() + "].ComentarioGestor", "O comentário do gestor é obrigatório");
                    }
                }
            }

            if (model.ListaCompetenciasFuncionais != null)
            {
                for (int i = 0; i < model.ListaCompetenciasFuncionais.Count; i++)
                {
                    if (model.ListaCompetenciasFuncionais[i].NivelColaborador != model.ListaCompetenciasFuncionais[i].NivelGestor
                        && string.IsNullOrEmpty(model.ListaCompetenciasFuncionais[i].ComentarioGestor))
                    {
                        ModelState.AddModelError("ListaCompetenciasFuncionais[" + i.ToString() + "].ComentarioGestor", "O comentário do gestor é obrigatório");
                    }
                }
            }

            if (model.ListaCompetenciasLideranca != null)
            {
                for (int i = 0; i < model.ListaCompetenciasLideranca.Count; i++)
                {
                    if (model.ListaCompetenciasLideranca[i].NivelColaborador != model.ListaCompetenciasLideranca[i].NivelGestor
                        && string.IsNullOrEmpty(model.ListaCompetenciasLideranca[i].ComentarioGestor))
                    {
                        ModelState.AddModelError("ListaCompetenciasLideranca[" + i.ToString() + "].ComentarioGestor", "O comentário do gestor é obrigatório");
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                var identidade = new Identidade();

                int usuarioID = identidade.UsuarioID;

                model.UsuarioRubiID = identidade.UsuarioRubiID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasCorporativas != null)
                    {
                        foreach (var item in model.ListaCompetenciasCorporativas)
                        {
                            if (model.AcessoGestor)
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador,
                                    NivelGestor = item.NivelGestor,
                                    NivelRequerido = item.NivelRequerido,
                                    ComentariosGestor = item.ComentarioGestor
                                });
                            }
                            else
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador
                                });
                            }
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }

                    competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasFuncionais != null)
                    {
                        foreach (var item in model.ListaCompetenciasFuncionais)
                        {
                            if (model.AcessoGestor)
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador,
                                    NivelGestor = item.NivelGestor,
                                    NivelRequerido = item.NivelRequerido,
                                    ComentariosGestor = item.ComentarioGestor
                                });
                            }
                            else
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador
                                });
                            }
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }

                    competenciasCorporativas = new List<CompetenciaColaborador>();

                    if (model.ListaCompetenciasLideranca != null)
                    {
                        foreach (var item in model.ListaCompetenciasLideranca)
                        {
                            if (model.AcessoGestor)
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador,
                                    NivelGestor = item.NivelGestor,
                                    NivelRequerido = item.NivelRequerido,
                                    ComentariosGestor = item.ComentarioGestor
                                });
                            }
                            else
                            {
                                competenciasCorporativas.Add(new CompetenciaColaborador
                                {
                                    ID = item.ID.Value,
                                    CompetenciaID = item.CompentenciaID,
                                    AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value,
                                    NivelColaborador = item.NivelColaborador
                                });
                            }
                        }

                        new CompetenciaColaboradorDAO().PersistirColecao(competenciasCorporativas);
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;

                model.CicloAvaliacaoSelecionadoID = model.CicloAvaliacaoSelecionadoID.Value;

                return ManterAvaliacaoColaboradorCompetenciasGestor(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID);
            }

            var listaAval = new List<SelectListItem>();

            for (int i = 0; i < 5; i++)
            {
                listaAval.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            model.ListaNivelAvaliacao = listaAval;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorCompetenciasGestor.cshtml", model);
        }

        #endregion Competências

        #region Performance
        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorPerformance(int? id,
                                                                   int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorPerformanceViewModel model =
                new ManterAvaliacaoColaboradorPerformanceViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = usuarioID;

            model.PapelID = identidade.PapelID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAvaliacaoGestor = cicloAvaliacao.DataInicioAvaliacaoGestor;
                model.DataTerminoAvaliacaoGestor = cicloAvaliacao.DataTerminoAvaliacaoGestor;
                model.StatusCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var performanceColaborador = new PerformanceColaboradorDAO().Obter(model.AvaliacaoColaboradorID.Value);

                    model.AvaliacaoPerformanceGerais = new ItemListaPerformanceColaborador();

                    if (performanceColaborador != null)
                    {
                        model.AvaliacaoPerformanceGerais.ID = performanceColaborador.ID;
                        model.AvaliacaoPerformanceGerais.AvaliacaoPerformanceGeral = performanceColaborador.AvaliacaoPerformance;
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorPerformance.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorPerformance(ManterAvaliacaoColaboradorPerformanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identidade = new Identidade();

                int usuarioID = identidade.UsuarioID;

                model.UsuarioRubiID = identidade.UsuarioRubiID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var competenciasCorporativas = new List<CompetenciaColaborador>();

                    PerformanceColaboradorDAO performanceColaboradorDAO = new PerformanceColaboradorDAO();

                    var performanceColaborador = performanceColaboradorDAO.Obter(model.AvaliacaoColaboradorID.Value);

                    if (performanceColaborador == null)
                    {
                        performanceColaborador = new PerformanceColaborador();
                        performanceColaborador.AvaliacaoPerformance = model.AvaliacaoPerformanceGerais.AvaliacaoPerformanceGeral;
                        performanceColaborador.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;

                        performanceColaboradorDAO.Incluir(performanceColaborador);
                    }
                    else
                    {
                        performanceColaborador.AvaliacaoPerformance = model.AvaliacaoPerformanceGerais.AvaliacaoPerformanceGeral;

                        performanceColaboradorDAO.Editar(performanceColaborador);
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;

                model.CicloAvaliacaoSelecionadoID = model.CicloAvaliacaoSelecionadoID.Value;
            }

            return ManterAvaliacaoColaboradorPerformance(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID);
        }
        #endregion Performance

        #region Recomendação
        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorRecomendacao(int? id,
                                                                   int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorRecomendacaoViewModel model =
                new ManterAvaliacaoColaboradorRecomendacaoViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = usuarioID;

            model.PapelID = identidade.PapelID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAvaliacaoGestor = cicloAvaliacao.DataInicioAvaliacaoGestor;
                model.DataTerminoAvaliacaoGestor = cicloAvaliacao.DataTerminoAvaliacaoGestor;
                model.StatusCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    if (model.PapelID == (int)Enumeradores.CodigoPapeis.Administrador
                    && (avaliacaoColaborador.StatusAvaliacaoColaborador_ID.Equals((int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH)
                        || avaliacaoColaborador.StatusAvaliacaoColaborador_ID.Equals((int)Enumeradores.StatusAvaliacaoColaborador.Encerrada)))
                    {
                        return ManterAvaliacaoColaboradorRecomendacaoRH(id.Value, model.ColaboradorID);
                    }

                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var recomendacaoColaborador = new RecomendacaoColaboradorDAO().Obter(model.AvaliacaoColaboradorID.Value);

                    model.ItemRecomendacaoColaborador = new ItemListaRecomendacaoColaborador();

                    if (recomendacaoColaborador != null)
                    {
                        model.ItemRecomendacaoColaborador.RecomendacaoDeRating = recomendacaoColaborador.RecomendacaoDeRating;
                        model.ItemRecomendacaoColaborador.RecomendacaoDePromocao = recomendacaoColaborador.RecomendacaoDePromocao;
                        model.ItemRecomendacaoColaborador.Justificativa = recomendacaoColaborador.Justificativa;
                        model.ItemRecomendacaoColaborador.JustificativaDaJustificativa = recomendacaoColaborador.JustificativaDaJustificativa;

                        //if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH)
                        //{
                        //    model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem =  recomendacaoColaborador.RatingFinalPosCalibragem;
                        //    model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem = recomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                        //    model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem = recomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                        //    model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        //}
                    }

                    var listaAval = new List<SelectListItem>();

                    listaAval.Add(new SelectListItem() { Text = "Excepcional", Value = "1" });
                    listaAval.Add(new SelectListItem() { Text = "Excede as Expectativas", Value = "2" });
                    listaAval.Add(new SelectListItem() { Text = "Atende as Expectativas", Value = "3" });
                    listaAval.Add(new SelectListItem() { Text = "Abaixo das Expectativas", Value = "4" });

                    model.ListaRecomendacaoDeRating = listaAval;

                    listaAval = new List<SelectListItem>();

                    listaAval.Add(new SelectListItem() { Text = "Sim", Value = "true" });
                    listaAval.Add(new SelectListItem() { Text = "Não", Value = "false" });

                    model.ListaSimNao = listaAval;
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorRecomendacao.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorRecomendacao(ManterAvaliacaoColaboradorRecomendacaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identidade = new Identidade();

                int usuarioID = identidade.UsuarioID;

                model.UsuarioRubiID = identidade.UsuarioRubiID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    RecomendacaoColaboradorDAO recomendacaoColaboradorDAO = new RecomendacaoColaboradorDAO();

                    var recomendacaoColaborador = recomendacaoColaboradorDAO.Obter(model.AvaliacaoColaboradorID.Value);

                    if (recomendacaoColaborador == null)
                    {
                        recomendacaoColaborador = new RecomendacaoColaborador();
                        recomendacaoColaborador.AvaliacaoColaborador_ID = model.AvaliacaoColaboradorID.Value;
                        recomendacaoColaborador.RecomendacaoDeRating = model.ItemRecomendacaoColaborador.RecomendacaoDeRating;
                        recomendacaoColaborador.RecomendacaoDePromocao = model.ItemRecomendacaoColaborador.RecomendacaoDePromocao.Value;
                        recomendacaoColaborador.Justificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.JustificativaDaJustificativa;

                        //if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH)
                        //{
                        //    recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        //    recomendacaoColaborador.RatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem;
                        //    recomendacaoColaborador.IndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                        //    recomendacaoColaborador.JustificativaRatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                        //    recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        //}

                        recomendacaoColaboradorDAO.Incluir(recomendacaoColaborador);
                    }
                    else
                    {
                        recomendacaoColaborador.RecomendacaoDeRating = model.ItemRecomendacaoColaborador.RecomendacaoDeRating;
                        recomendacaoColaborador.RecomendacaoDePromocao = model.ItemRecomendacaoColaborador.RecomendacaoDePromocao.Value;
                        recomendacaoColaborador.Justificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.JustificativaDaJustificativa;

                        //if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH)
                        //{
                        //    recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        //    recomendacaoColaborador.RatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem;
                        //    recomendacaoColaborador.IndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                        //    recomendacaoColaborador.JustificativaRatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                        //    recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        //}

                        recomendacaoColaboradorDAO.Editar(recomendacaoColaborador);
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;

                model.CicloAvaliacaoSelecionadoID = model.CicloAvaliacaoSelecionadoID.Value;

                return ManterAvaliacaoColaboradorRecomendacao(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID);
            }

            var listaAval = new List<SelectListItem>();

            listaAval.Add(new SelectListItem() { Text = "Excepcional", Value = "1" });
            listaAval.Add(new SelectListItem() { Text = "Excede as Expectativas", Value = "2" });
            listaAval.Add(new SelectListItem() { Text = "Atende as Expectativas", Value = "3" });
            listaAval.Add(new SelectListItem() { Text = "Abaixo das Expectativas", Value = "4" });

            model.ListaRecomendacaoDeRating = listaAval;

            listaAval = new List<SelectListItem>();

            listaAval.Add(new SelectListItem() { Text = "Sim", Value = "true" });
            listaAval.Add(new SelectListItem() { Text = "Não", Value = "false" });

            model.ListaSimNao = listaAval;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorRecomendacao.cshtml", model);
        }

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        public ActionResult ManterAvaliacaoColaboradorRecomendacaoRH(int? id,
                                                                     int? colaboradorID = null)
        {
            ManterAvaliacaoColaboradorRecomendacaoRHViewModel model =
                new ManterAvaliacaoColaboradorRecomendacaoRHViewModel();

            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

            model.UsuarioRubiID = identidade.UsuarioRubiID;

            model.ColaboradorID = usuarioID;

            model.PapelID = identidade.PapelID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(id.Value);

            if (cicloAvaliacao != null)
            {
                model.DataInicioAvaliacaoGestor = cicloAvaliacao.DataInicioAvaliacaoGestor;
                model.DataTerminoAvaliacaoGestor = cicloAvaliacao.DataTerminoAvaliacaoGestor;
                model.StatusCicloAvaliacaoID = cicloAvaliacao.SituacaoCicloAvaliacao_ID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(id.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    var recomendacaoColaborador = new RecomendacaoColaboradorDAO().Obter(model.AvaliacaoColaboradorID.Value);

                    model.ItemRecomendacaoColaborador = new ItemListaRecomendacaoColaboradorRH();

                    if (recomendacaoColaborador != null)
                    {
                        model.ItemRecomendacaoColaborador.RecomendacaoDeRating = recomendacaoColaborador.RecomendacaoDeRating;
                        model.ItemRecomendacaoColaborador.RecomendacaoDePromocao = recomendacaoColaborador.RecomendacaoDePromocao;
                        model.ItemRecomendacaoColaborador.Justificativa = recomendacaoColaborador.Justificativa;
                        model.ItemRecomendacaoColaborador.JustificativaDaJustificativa = recomendacaoColaborador.JustificativaDaJustificativa;

                        if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH ||
                            model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.Encerrada)
                        {
                            model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem = recomendacaoColaborador.RatingFinalPosCalibragem;
                            model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem = recomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                            model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem = recomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                            model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        }
                    }

                    var listaAval = new List<SelectListItem>();

                    listaAval.Add(new SelectListItem() { Text = "Excepcional", Value = "1" });
                    listaAval.Add(new SelectListItem() { Text = "Excede as Expectativas", Value = "2" });
                    listaAval.Add(new SelectListItem() { Text = "Atende as Expectativas", Value = "3" });
                    listaAval.Add(new SelectListItem() { Text = "Abaixo das Expectativas", Value = "4" });

                    model.ListaRecomendacaoDeRating = listaAval;

                    listaAval = new List<SelectListItem>();

                    listaAval.Add(new SelectListItem() { Text = "Sim", Value = "true" });
                    listaAval.Add(new SelectListItem() { Text = "Não", Value = "false" });

                    model.ListaSimNao = listaAval;
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;
            }

            model.CicloAvaliacaoSelecionadoID = id.Value;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorRecomendacaoRH.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel))]
        [CriacaoMapeamento(typeof(DeContribuicaoColaboradorParaOutrasContribuicoesViewModel))]
        [CriacaoMapeamento(typeof(DeStatusAvaliacaoColaboradorParaSelectListItem))]
        public ActionResult ManterAvaliacaoColaboradorRecomendacaoRH(ManterAvaliacaoColaboradorRecomendacaoRHViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identidade = new Identidade();

                int usuarioID = identidade.UsuarioID;

                model.UsuarioRubiID = identidade.UsuarioRubiID;

                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacaoColaborador != null)
                {
                    model.GestorRubiID = avaliacaoColaborador.GestorRubi_ID;
                    model.GestorRubiEmpID = avaliacaoColaborador.GestorRubiEmp_ID;

                    model.AvaliacaoColaboradorID = avaliacaoColaborador.ID;

                    model.StatusAvaliacaoColaboradorID = avaliacaoColaborador.StatusAvaliacaoColaborador_ID;

                    RecomendacaoColaboradorDAO recomendacaoColaboradorDAO = new RecomendacaoColaboradorDAO();

                    var recomendacaoColaborador = recomendacaoColaboradorDAO.Obter(model.AvaliacaoColaboradorID.Value);

                    if (recomendacaoColaborador != null)
                    {
                        recomendacaoColaborador.RecomendacaoDeRating = model.ItemRecomendacaoColaborador.RecomendacaoDeRating.Value;
                        recomendacaoColaborador.RecomendacaoDePromocao = model.ItemRecomendacaoColaborador.RecomendacaoDePromocao;
                        recomendacaoColaborador.Justificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.JustificativaDaJustificativa;

                        if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH ||
                            model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.Encerrada)
                        {
                            recomendacaoColaborador.RatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem;
                            recomendacaoColaborador.IndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                            recomendacaoColaborador.JustificativaRatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                            recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        }

                        recomendacaoColaboradorDAO.Editar(recomendacaoColaborador);
                    }
                    else
                    {
                        recomendacaoColaborador = new RecomendacaoColaborador();
                        recomendacaoColaborador.AvaliacaoColaborador_ID = avaliacaoColaborador.ID;
                        recomendacaoColaborador.RecomendacaoDeRating = model.ItemRecomendacaoColaborador.RecomendacaoDeRating;
                        recomendacaoColaborador.RecomendacaoDePromocao = model.ItemRecomendacaoColaborador.RecomendacaoDePromocao;
                        recomendacaoColaborador.Justificativa = model.ItemRecomendacaoColaborador.Justificativa;
                        recomendacaoColaborador.JustificativaDaJustificativa = model.ItemRecomendacaoColaborador.JustificativaDaJustificativa;

                        if (model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.EmAvaliacaoPelosRH ||
                            model.StatusAvaliacaoColaboradorID == (int)Enumeradores.StatusAvaliacaoColaborador.Encerrada)
                        {
                            recomendacaoColaborador.RatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem;
                            recomendacaoColaborador.IndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                            recomendacaoColaborador.JustificativaRatingFinalPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                            recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                        }

                        recomendacaoColaboradorDAO.Incluir(recomendacaoColaborador);
                    }
                }
                else
                    model.GestorRubiID = identidade.GestorRubiID;

                model.CicloAvaliacaoSelecionadoID = model.CicloAvaliacaoSelecionadoID.Value;

                return GestaoAvaliacoesColaboradores(model.CicloAvaliacaoSelecionadoID);
            }

            var listaAval = new List<SelectListItem>();

            listaAval.Add(new SelectListItem() { Text = "Excepcional", Value = "1" });
            listaAval.Add(new SelectListItem() { Text = "Excede as Expectativas", Value = "2" });
            listaAval.Add(new SelectListItem() { Text = "Atende as Expectativas", Value = "3" });
            listaAval.Add(new SelectListItem() { Text = "Abaixo das Expectativas", Value = "4" });

            model.ListaRecomendacaoDeRating = listaAval;

            listaAval = new List<SelectListItem>();

            listaAval.Add(new SelectListItem() { Text = "Sim", Value = "true" });
            listaAval.Add(new SelectListItem() { Text = "Não", Value = "false" });

            model.ListaSimNao = listaAval;

            return View("~/Views/Avaliacoes/ManterAvaliacaoColaboradorRecomendacaoRH.cshtml", model);
        }

        #endregion Recomendação

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

            model.ColaboradorID = usuarioID;

            #region <<<<< Validar permissão de usuário >>>>>
            if (identidade.UsuarioID != usuarioID)
            {
                if (identidade.PapelID != (int)Enumeradores.CodigoPapeis.Administrador)
                {
                    if (!new UsuarioDAO().IsGestor(usuarioID, identidade.UsuarioRubiID.Value))
                    {
                        return View("~/Views/Shared/PaginaNaoEncontrada.cshtml");
                    }
                }
            }
            #endregion

            var avaliacaoPDIColaborador =
                new AvaliacaoPDIColaboradorDAO().Obter(id.Value, usuarioID);

            if (avaliacaoPDIColaborador != null)
            {
                model.GestorRubiID = avaliacaoPDIColaborador.GestorRubi_ID;
                model.GestorRubiEmpID = avaliacaoPDIColaborador.GestorRubiEmp_ID;

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
            {
                model.GestorRubiID = identidade.GestorRubiID;
                model.GestorRubiEmpID = identidade.CodigoEmpresaRubiUD;
                model.AreaRubiID = identidade.AreaRubiID;
                model.SetorRubiID = identidade.SetorRubiID;
                model.CargoRubiID = identidade.CargoRubiID;
            }

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
                    avaliacaoPDI.GestorRubi_ID = identidade.GestorRubiID;
                    avaliacaoPDI.GestorRubiEmp_ID = identidade.CodigoEmpresaRubiUD;
                    avaliacaoPDI.StatusPDI_ID = (int)Enumeradores.StatusPDI.Criada;
                    avaliacaoPDI.Idiomas = model.Idiomas;
                    avaliacaoPDI.Graduacao = model.Graduacao;
                    avaliacaoPDI.PontosFortes = model.PontosFortes;
                    avaliacaoPDI.PontosDesenvolvimento = model.PontosDesenvolvimento;
                    avaliacaoPDI.ComentariosColaborador = model.ComentariosColaborador;
                    avaliacaoPDI.AreaRubiID = identidade.AreaRubiID;
                    avaliacaoPDI.CargoRubiID = identidade.CargoRubiID;
                    avaliacaoPDI.SetorRubiID = identidade.SetorRubiID;

                    avaliacaoPDIColaboradorDAO.Incluir(avaliacaoPDI);

                    model.AvaliacaoPDIColaboradorID = avaliacaoPDI.ID;
                }
                else
                {
                    var avaliacaoPDI = avaliacaoPDIColaboradorDAO.Obter(model.CicloAvaliacaoSelecionadoID.Value, new Identidade().UsuarioID);

                    if (avaliacaoPDI != null)
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
                    desenvolvimentoCompetencia.RecursoSuporte = model.DesenvolvimentoCompetenciaCadastro.RecursoSuporte;

                    desenvolvimentoCompetenciaDAO.Incluir(desenvolvimentoCompetencia);
                }

                if (model.ListaDesenvolvimentoCompetenciaViewModel != null)
                {
                    foreach (var item in model.ListaDesenvolvimentoCompetenciaViewModel)
                    {
                        var desenvolvimentoCompetencia = desenvolvimentoCompetenciaDAO.Obter(item.ID);

                        desenvolvimentoCompetencia.AcaoDesenvolvimento = item.AcaoDesenvolvimento;
                        desenvolvimentoCompetencia.RecursoSuporte = item.RecursoSuporte;

                        desenvolvimentoCompetenciaDAO.Editar(desenvolvimentoCompetencia);
                    }
                }
            }

            return ManterAvaliacaoPDIColaborador(model.CicloAvaliacaoSelecionadoID);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SalvarComentarioGestor(ManterAvaliacaoPDIColaboradorViewModel model)
        {
            if (model.ComentariosGestor == null)
            {
                ModelState.AddModelError("ComentariosGestor", "O comentário do gestor é obrigatório");
            }
            if (ModelState.IsValid)
            {
                AvaliacaoPDIColaboradorDAO avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();
                var avaliacao = avaliacaoPDIColaboradorDAO.Obter(model.CicloAvaliacaoSelecionadoID.Value, model.ColaboradorID.Value);

                if (avaliacao != null)
                {
                    avaliacao.ComentariosGestor = model.ComentariosGestor;
                    avaliacaoPDIColaboradorDAO.Editar(avaliacao);
                }
            }
            
            return View("~/Views/Avaliacoes/ManterAvaliacaoPDIColaborador.cshtml", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AprovarAvaliacaoPDIColaborador(int? cicloAvaliacaoSelecionadoID, int? usuarioID)
        {
            return AprovarReprovarAvaliacaoPDIColaborador(cicloAvaliacaoSelecionadoID, usuarioID, true, string.Empty);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ReprovarAvaliacaoPDIColaborador(ManterAvaliacaoPDIColaboradorViewModel model)
        {
            if (model.ComentariosGestor == null)
            {
                ModelState.AddModelError("ComentariosGestor", "O comentário do gestor é obrigatório");
            }
            if (ModelState.IsValid)
            {
                return AprovarReprovarAvaliacaoPDIColaborador(model.CicloAvaliacaoSelecionadoID, model.ColaboradorID, false, model.ComentariosGestor);
            }
            else
            {
                return View("~/Views/Avaliacoes/ManterAvaliacaoPDIColaborador.cshtml", model);
            }
        }

        private ActionResult AprovarReprovarAvaliacaoPDIColaborador(int? cicloAvaliacaoSelecionadoID,
                                                                 int? usuarioID,
                                                                 bool aprovada,
                                                                 string ComentariosGestor)
        {
            AvaliacaoPDIColaboradorDAO avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();

            var avaliacao = avaliacaoPDIColaboradorDAO.Obter(cicloAvaliacaoSelecionadoID.Value, usuarioID.Value);

            if (avaliacao != null)
            {
                avaliacao.Aprovada = aprovada;

                if (!aprovada)
                {
                    avaliacao.StatusPDI_ID = (int)Enumeradores.StatusPDI.Criada;
                    avaliacao.StatusPDI = new StatusPDIDAO().Obter((int)Enumeradores.StatusPDI.Criada);
                    avaliacao.ComentariosGestor = ComentariosGestor;
                }
                else
                {
                    avaliacao.StatusPDI_ID = (int)Enumeradores.StatusPDI.Aprovada;
                    avaliacao.StatusPDI = new StatusPDIDAO().Obter((int)Enumeradores.StatusPDI.Aprovada);
                    avaliacao.ComentariosGestor = string.Empty;
                }

                avaliacaoPDIColaboradorDAO.Editar(avaliacao);
            }

            return CarregarGestaoAvaliacaoColaborador(cicloAvaliacaoSelecionadoID, new Identidade());
        }

        #endregion
    }
}