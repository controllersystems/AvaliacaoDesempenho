﻿using AutoMapper;
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
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using AvaliacaoDesempenho.Seguranca;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Net.Mail;
using System.Net;
using System.Configuration;

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
            
            model.SituacoesCicloAvaliacao
                    = Mapper.Map<List<SituacaoCicloAvaliacao>,
                                 List<SelectListItem>>(new SituacaoCicloAvaliacaoDAO().Listar());

            model.AcaoPagina = acaoPagina;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [CriacaoMapeamento(typeof(DeManterCicloAvaliacaoViewModelParaCicloAvaliacao))]
        [CriacaoMapeamento(typeof(DeSituacaoCicloAvaliacaoParaSelectListItem))]
        public ActionResult ManterCicloAvaliacao(ManterCicloAvaliacaoViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DataInicioVigencia) && !string.IsNullOrEmpty(model.DataFimVigencia))
            {
                if (model.ID.HasValue)
                {
                    var ciclosAfetados = new CicloAvaliacaoDAO().ListarCiclosSobrepostosQuePossuemEssaData(Convert.ToDateTime(model.DataInicioVigencia), model.ID.Value);
                    if (ciclosAfetados != null && ciclosAfetados.Any())
                    {
                        ModelState.AddModelError("DataInicioVigencia", "A Data de Início conflita com um intervalo de vigência de outro ciclo de avaliação.");
                    }
                    ciclosAfetados = new CicloAvaliacaoDAO().ListarCiclosSobrepostosQuePossuemEssaData(Convert.ToDateTime(model.DataFimVigencia), model.ID.Value);
                    if (ciclosAfetados != null && ciclosAfetados.Any())
                    {
                        ModelState.AddModelError("DataFimVigencia", "A Data de Fim conflita com um intervalo de vigência de outro ciclo de avaliação.");
                    }
                }
                else
                {
                    var ciclosAfetados = new CicloAvaliacaoDAO().ListarCiclosSobrepostosQuePossuemEssaData(Convert.ToDateTime(model.DataInicioVigencia));
                    if (ciclosAfetados != null && ciclosAfetados.Any())
                    {
                        ModelState.AddModelError("DataInicioVigencia", "A Data de Início conflita com um intervalo de vigência de outro ciclo de avaliação.");
                    }
                    ciclosAfetados = new CicloAvaliacaoDAO().ListarCiclosSobrepostosQuePossuemEssaData(Convert.ToDateTime(model.DataFimVigencia));
                    if (ciclosAfetados != null && ciclosAfetados.Any())
                    {
                        ModelState.AddModelError("DataFimVigencia", "A Data de Fim conflita com um intervalo de vigência de outro ciclo de avaliação.");
                    }
                }
                if (Convert.ToDateTime(model.DataInicioVigencia) > Convert.ToDateTime(model.DataFimVigencia))
                {
                    ModelState.AddModelError("DataFimVigencia", "A Data de Término tem que ser maior que a data de Início.");
                }
            }

            if (string.IsNullOrEmpty(model.DataInicioObjetivosMetas) && !string.IsNullOrEmpty(model.DataTerminoObjetivosMetas))
            {
                ModelState.AddModelError("DataInicioObjetivosMetas", "A Data de Início de Definição de Objetivos e Metas deve ser informada.");
            }
            else
            {
                if (!string.IsNullOrEmpty(model.DataInicioObjetivosMetas) && string.IsNullOrEmpty(model.DataTerminoObjetivosMetas))
                {
                    ModelState.AddModelError("DataTerminoObjetivosMetas", "A Data de Término de Definição de Objetivos e Metas deve ser informada.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DataInicioObjetivosMetas) && !string.IsNullOrEmpty(model.DataTerminoObjetivosMetas))
                    {
                        if (Convert.ToDateTime(model.DataInicioObjetivosMetas) > Convert.ToDateTime(model.DataTerminoObjetivosMetas))
                        {
                            ModelState.AddModelError("DataTerminoObjetivosMetas", "A Data de Término tem que ser maior que a data de Início.");
                        }

                        if (!string.IsNullOrEmpty(model.DataInicioVigencia) && !string.IsNullOrEmpty(model.DataFimVigencia))
                        {
                            if(Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataInicioObjetivosMetas) ||
                                Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataTerminoObjetivosMetas))
                            {
                                ModelState.AddModelError("DataInicioObjetivosMetas", "A Data de Definição de Objetivos e Metas está fora do período do ciclo de avaliação.");
                            }                                
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(model.DataInicioAutoAvaliacao) && !string.IsNullOrEmpty(model.DataTerminoAutoAvaliacao))
            {
                ModelState.AddModelError("DataInicioAutoAvaliacao", "A Data de Início da Auto Avaliação deve ser informada.");
            }
            else
            {
                if (!string.IsNullOrEmpty(model.DataInicioAutoAvaliacao) && string.IsNullOrEmpty(model.DataTerminoAutoAvaliacao))
                {
                    ModelState.AddModelError("DataTerminoAutoAvaliacao", "A Data de Término da Auto Avaliação deve ser informada.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DataInicioAutoAvaliacao) && !string.IsNullOrEmpty(model.DataTerminoAutoAvaliacao))
                    {
                        if (Convert.ToDateTime(model.DataInicioAutoAvaliacao) > Convert.ToDateTime(model.DataTerminoAutoAvaliacao))
                        {
                            ModelState.AddModelError("DataTerminoAutoAvaliacao", "A Data de Término tem que ser maior que a data de Início.");
                        }

                        if (!string.IsNullOrEmpty(model.DataInicioVigencia) && !string.IsNullOrEmpty(model.DataFimVigencia))
                        {
                            if (Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataInicioAutoAvaliacao) ||
                                Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataTerminoAutoAvaliacao))
                            {
                                ModelState.AddModelError("DataInicioAutoAvaliacao", "A Data da Auto Avaliação está fora do período do ciclo de avaliação.");
                            }
                        }

                        if (!string.IsNullOrEmpty(model.DataTerminoObjetivosMetas))
                        {
                            if (Convert.ToDateTime(model.DataTerminoObjetivosMetas) > Convert.ToDateTime(model.DataInicioAutoAvaliacao))
                            {
                                ModelState.AddModelError("DataInicioAutoAvaliacao", "A Data de Início da Auto Avaliação tem que ser maior ou igual a data de Término de Definição de Objetivos e Metas.");
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(model.DataInicioAvaliacaoGestor) && !string.IsNullOrEmpty(model.DataTerminoAvaliacaoGestor))
            {
                ModelState.AddModelError("DataInicioAvaliacaoGestor", "A Data de Início da Avaliação do Gestor deve ser informada.");
            }
            else
            {
                if (!string.IsNullOrEmpty(model.DataInicioAvaliacaoGestor) && string.IsNullOrEmpty(model.DataTerminoAvaliacaoGestor))
                {
                    ModelState.AddModelError("DataTerminoAvaliacaoGestor", "A Data de Término da Avaliação do Gestor deve ser informada.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DataInicioAvaliacaoGestor) && !string.IsNullOrEmpty(model.DataTerminoAvaliacaoGestor))
                    {
                        if (Convert.ToDateTime(model.DataInicioAvaliacaoGestor) > Convert.ToDateTime(model.DataTerminoAvaliacaoGestor))
                        {
                            ModelState.AddModelError("DataTerminoAvaliacaoGestor", "A Data de Término tem que ser maior que a data de Início.");
                        }

                        if (!string.IsNullOrEmpty(model.DataInicioVigencia) && !string.IsNullOrEmpty(model.DataFimVigencia))
                        {
                            if (Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataInicioAvaliacaoGestor) ||
                                Convert.ToDateTime(model.DataFimVigencia) < Convert.ToDateTime(model.DataTerminoAvaliacaoGestor))
                            {
                                ModelState.AddModelError("DataInicioAutoAvaliacao", "A Data da Avaliação do Gestor está fora do período do ciclo de avaliação.");
                            }
                        }

                        if (!string.IsNullOrEmpty(model.DataTerminoAutoAvaliacao))
                        {
                            if (Convert.ToDateTime(model.DataTerminoAutoAvaliacao) > Convert.ToDateTime(model.DataInicioAvaliacaoGestor))
                            {
                                ModelState.AddModelError("DataInicioAvaliacaoGestor", "A Data de Início da Avaliação do Gestor tem que ser maior ou igual a data de Término da Auto Avaliação.");
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(model.DataInicioGerenciamentoPDI) && !string.IsNullOrEmpty(model.DataTerminoGerenciamentoPDI))
            {
                ModelState.AddModelError("DataInicioGerenciamentoPDI", "A Data de Início de Gerenciamento de PDI deve ser informada.");
            }
            else
            {
                if (!string.IsNullOrEmpty(model.DataInicioGerenciamentoPDI) && string.IsNullOrEmpty(model.DataTerminoGerenciamentoPDI))
                {
                    ModelState.AddModelError("DataTerminoGerenciamentoPDI", "A Data de Término de Gerenciamento de PDI deve ser informada.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DataInicioGerenciamentoPDI) && !string.IsNullOrEmpty(model.DataTerminoGerenciamentoPDI))
                    {
                        if (Convert.ToDateTime(model.DataInicioGerenciamentoPDI) > Convert.ToDateTime(model.DataTerminoGerenciamentoPDI))
                        {
                            ModelState.AddModelError("DataTerminoGerenciamentoPDI", "A Data de Término tem que ser maior que a data de Início.");
                        }
                    }
                }
            }

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
        [CriacaoMapeamento(typeof(DeListAssociacaoCargoCompetenciaParaListItemListaGestaoCompetenciasCargosViewModel))]
        [CriacaoMapeamento(typeof(DeUSU_V092ESTParaAssociacaoParaAssociacaoCargoCompetenciaViewModel))]
        [CriacaoMapeamento(typeof(Detbl_cargo_sccParaSelectListItem))]
        [CriacaoMapeamento(typeof(Detbl_area_sccParaSelectListItem))]
        [CriacaoMapeamento(typeof(Detbl_setor_sccParaSelectListItem))]
        public ActionResult GestaoCompetenciasCargos(int? id, int? pagina = 1)
        {
            GestaoCompentenciasCargosViewModel model = new GestaoCompentenciasCargosViewModel();

            model.CicloAvaliacaoSelecionadoID = id;

            model.Pagina = pagina.Value;

            var ciclo = new CicloAvaliacaoDAO().Obter(id.Value);

            if (ciclo != null)
            {
                model.ExisteUltimoCiclo = new CicloAvaliacaoDAO().ExisteCicloAnteriorEncerrado(id.Value, ciclo.DataInicioVigencia);
            }

            CarregarAssociacoesCargoCompetencias(model);

            return View("/Views/CiclosAvaliacao/GestaoCompetenciasCargos.cshtml", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ConsolidarCargosRubi(int? id)
        {
            GestaoCompentenciasCargosViewModel model = new GestaoCompentenciasCargosViewModel();

            model.CicloAvaliacaoSelecionadoID = id;

            CarregarInformacoesRubi(model);

            AssociacaoCargoCompetenciaDAO associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

            if (model.AssociacoesCargosCompetencias != null)
            {                
                var associacoesCargoCompetencia = new List<AssociacaoCargoCompetencia>();

                foreach (var item in model.AssociacoesCargosCompetencias)
                {
                    var associacao = associacaoCargoCompetenciaDAO.Obter(id.Value, item.CargoRubiID, item.AreaRubiID, item.SetorRubiID.Value);

                    if (associacao == null)
                    {
                        associacoesCargoCompetencia.Add(new AssociacaoCargoCompetencia
                        {
                            AreaCompetencia = item.AreaCompetencia,
                            AreaCompetenciaID = item.AreaCompetenciaID,
                            AreaRubi = item.AreaRubi,
                            AreaRubiID = item.AreaRubiID,
                            CargoCompetencia = item.CargoCompetencia,
                            CargoCompetenciaID = item.CargoCompetenciaID,
                            CargoRubi = item.CargoRubi,
                            CargoRubiID = item.CargoRubiID,
                            CicloAvaliacao_ID = id.Value,
                            ID = item.ID,
                            SetorCompetencia = item.SetorCompetencia,
                            SetorCompetenciaID = item.SetorCompetenciaID,
                            SetorRubi = item.SetorRubi,
                            SetorRubiID = item.SetorRubiID
                        });
                    }
                }

                associacaoCargoCompetenciaDAO.PersistirColecao(associacoesCargoCompetencia);
            }

            CarregarAssociacoesCargoCompetencias(model);

            model.Pagina = 1;

            return View("/Views/CiclosAvaliacao/GestaoCompetenciasCargos.cshtml", model);
        }

        private void CadastrarUsuario(string login, int numEmp, int numCad)
        {
            IntegracaoRubi integracaoRubi = new IntegracaoRubi();
            UsuarioDAO usuarioDAO = new UsuarioDAO();

            var usuarioRubi = integracaoRubi.ObterUSU_V034FAD(numEmp, numCad);

            var usuario = new Usuario()
            {
                Nome = usuarioRubi.NOMFUN,
                Email = usuarioRubi.EMACOM,
                Login = login,
                Papel_ID = (int)Enumeradores.CodigoPapeis.Colaborador,
                GestorRubiID = usuarioRubi.USU_LD1CAD,
                UsuarioRubiID = usuarioRubi.NUMCAD,
                CodigoEmpresaRubiUD = usuarioRubi.NUMEMP
            };

            usuarioDAO.Incluir(usuario);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        //[CriacaoMapeamento(typeof(DeItemListaGestaoCompetenciasCargosViewModelParaAssociacaoCargoCompetencia))]
        //[CriacaoMapeamento(typeof(DeListaItemListaGestaoCompetenciasCargosViewModelParaListaAssociacaoCargoCompetencia))]
        public ActionResult GestaoCompetenciasCargos(GestaoCompentenciasCargosViewModel model)
        {
            if (ModelState.IsValid)
            {
                AssociacaoCargoCompetenciaDAO associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

                CarregarInformacoesSistemaCompentencias(model);

                if (model.AssociacoesCargosCompetencias != null)
                {
                    for (int i = (model.Pagina - 1) * 20; i < (((model.Pagina * 20) > model.AssociacoesCargosCompetencias.Count) ? model.AssociacoesCargosCompetencias.Count : (model.Pagina * 20)); i++)
                    {
                        if (model.AssociacoesCargosCompetencias[i].CargoCompetenciaID.HasValue)
                            model.AssociacoesCargosCompetencias[i].CargoCompetencia = model.CargosCompentencia.First(p => p.Value.Equals(model.AssociacoesCargosCompetencias[i].CargoCompetenciaID.Value.ToString())).Text;

                        if (model.AssociacoesCargosCompetencias[i].AreaCompetenciaID.HasValue)
                            model.AssociacoesCargosCompetencias[i].AreaCompetencia = model.AreasCompentencia.First(p => p.Value.Equals(model.AssociacoesCargosCompetencias[i].AreaCompetenciaID.ToString())).Text;

                        if (model.AssociacoesCargosCompetencias[i].SetorCompetenciaID.HasValue)
                            model.AssociacoesCargosCompetencias[i].SetorCompetencia = model.SetoresCompentencia.First(p => p.Value.Equals(model.AssociacoesCargosCompetencias[i].SetorCompetenciaID.ToString())).Text;

                        model.AssociacoesCargosCompetencias[i].CicloAvaliacaoID = model.CicloAvaliacaoSelecionadoID.Value;
                    }

                    var cicloAvaliacaoDAO = new CicloAvaliacaoDAO();
                    CicloAvaliacao ciclo = cicloAvaliacaoDAO.Obter(model.CicloAvaliacaoSelecionadoID.Value);

                    var associacoesCargoCompetencia = new List<AssociacaoCargoCompetencia>();

                    for (int i = (model.Pagina - 1) * 20; i < (((model.Pagina * 20) > model.AssociacoesCargosCompetencias.Count) ? model.AssociacoesCargosCompetencias.Count : (model.Pagina * 20)); i++)
                    {
                        associacoesCargoCompetencia.Add(new AssociacaoCargoCompetencia
                            {
                                AreaCompetencia = model.AssociacoesCargosCompetencias[i].AreaCompetencia,
                                AreaCompetenciaID = model.AssociacoesCargosCompetencias[i].AreaCompetenciaID,
                                AreaRubi = model.AssociacoesCargosCompetencias[i].AreaRubi,
                                AreaRubiID = model.AssociacoesCargosCompetencias[i].AreaRubiID,
                                CargoCompetencia = model.AssociacoesCargosCompetencias[i].CargoCompetencia,
                                CargoCompetenciaID = model.AssociacoesCargosCompetencias[i].CargoCompetenciaID,
                                CargoRubi = model.AssociacoesCargosCompetencias[i].CargoRubi,
                                CargoRubiID = model.AssociacoesCargosCompetencias[i].CargoRubiID,
                                CicloAvaliacao_ID = model.AssociacoesCargosCompetencias[i].CicloAvaliacaoID,
                                ID = model.AssociacoesCargosCompetencias[i].ID,
                                SetorCompetencia = model.AssociacoesCargosCompetencias[i].SetorCompetencia,
                                SetorCompetenciaID = model.AssociacoesCargosCompetencias[i].SetorCompetenciaID,
                                SetorRubi = model.AssociacoesCargosCompetencias[i].SetorRubi,
                                SetorRubiID = model.AssociacoesCargosCompetencias[i].SetorRubiID
                            });
                    }

                    associacaoCargoCompetenciaDAO.PersistirColecao(associacoesCargoCompetencia);

                    #region <<<<< Criar as avaliações dos cargos que sofreram alterações. >>>>>
                    foreach (var item in associacoesCargoCompetencia)
                    {
                        if (item.AreaCompetenciaID.HasValue && item.CargoCompetenciaID.HasValue && item.SetorCompetenciaID.HasValue)
                        {
                            //Pega todos os usuarios do cargo, area e setor
                            var usuariosRubi = new IntegracaoRubi().ListarUSU_V034FAD(item.CargoRubiID, item.AreaRubiID, item.SetorRubiID.Value);

                            if (usuariosRubi != null)
                            {
                                foreach (var usuarioRubi in usuariosRubi)
                                {
                                    //Para cada usuario do rubi pegar seu respectivo usuario no competencias
                                    var usuarioCompetencia = new UsuarioDAO().Obter(usuarioRubi.NUMCAD, usuarioRubi.NUMEMP);

                                    //Caso não exista usuário no competencias, pegar as informações do AD e criar o usuário.
                                    if (usuarioCompetencia == null)
                                    {
                                        PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);

                                        DirectorySearcher directorySearcher = new DirectorySearcher(principalContext.ConnectedServer);

                                        directorySearcher.Filter = "(&(company=" + usuarioRubi.NUMEMP + ")(department=" + usuarioRubi.NUMCAD + ")" + System.Configuration.ConfigurationManager.ConnectionStrings["ADFilterConnectionString"].ConnectionString + ")";

                                        SearchResult searchResult = directorySearcher.FindOne();

                                        DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();

                                        if (directoryEntry.Properties.Count > 0)
                                        {
                                            CadastrarUsuario(directoryEntry.Properties["sAMAccountName"][0].ToString(), usuarioRubi.NUMEMP, usuarioRubi.NUMCAD);

                                            usuarioCompetencia = new UsuarioDAO().Obter(usuarioRubi.NUMCAD, usuarioRubi.NUMEMP);
                                        }
                                    }

                                    //Caso exista usuário cadastrado no sistema ou no AD, prosseguir pra criar a sua avaliação e o seu pdi.
                                    if (usuarioCompetencia != null)
                                    {
                                        //Verificar se existe seu gestor cadastrado no competencias, caso não exista cadastrar.
                                        var gestor = new UsuarioDAO().Obter(usuarioRubi.USU_LD1CAD.Value, usuarioRubi.USU_LD1EMP.Value);
                                        if (gestor == null)
                                        {
                                            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);

                                            DirectorySearcher directorySearcher = new DirectorySearcher(principalContext.ConnectedServer);

                                            directorySearcher.Filter = "(&(company=" + usuarioRubi.USU_LD1EMP.Value + ")(department=" + usuarioRubi.USU_LD1CAD.Value + ")" + System.Configuration.ConfigurationManager.ConnectionStrings["ADFilterConnectionString"].ConnectionString + ")";

                                            SearchResult searchResult = directorySearcher.FindOne();

                                            DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();

                                            if (directoryEntry.Properties.Count > 0)
                                            {
                                                CadastrarUsuario(directoryEntry.Properties["sAMAccountName"][0].ToString(), usuarioRubi.USU_LD1EMP.Value, usuarioRubi.USU_LD1CAD.Value);
                                            }
                                        }

                                        //Para cada usuario no competencia verificar se existe uma avaliação criada dentro desse ciclo
                                        var avaliacaoColaborador = new AvaliacaoColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, usuarioCompetencia.ID);

                                        if (avaliacaoColaborador == null)
                                        {
                                            if (ciclo.SituacaoCicloAvaliacao_ID.Value < 5)
                                            {
                                                AvaliacaoColaborador avaliacao = new AvaliacaoColaborador();

                                                AvaliacaoColaboradorDAO avaliacaoColaboradorDAO = new AvaliacaoColaboradorDAO();

                                                avaliacao.DataCriacao = DateTime.Today;
                                                avaliacao.CicloAvaliacao_ID = model.CicloAvaliacaoSelecionadoID.Value;
                                                avaliacao.Colaborador_ID = usuarioCompetencia.ID;
                                                avaliacao.GestorRubi_ID = usuarioRubi.USU_LD1CAD;
                                                avaliacao.GestorRubiEmp_ID = usuarioRubi.USU_LD1EMP;
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

                                                avaliacao.CargoRubiID = item.CargoRubiID;
                                                avaliacao.AreaRubiID = item.AreaRubiID;
                                                avaliacao.SetorRubiID = item.SetorRubiID;
                                                avaliacao.DataUltimaAlteracao = DateTime.Now;

                                                avaliacaoColaboradorDAO.Incluir(avaliacao);
                                            }
                                        }

                                        //Para cada usuario no competencia verificar se existe um pdi criado dentro desse ciclo
                                        var avaliacaoPdiColaborador = new AvaliacaoPDIColaboradorDAO().Obter(model.CicloAvaliacaoSelecionadoID.Value, usuarioCompetencia.ID);

                                        if (avaliacaoPdiColaborador == null)
                                        {
                                            if (ciclo.SituacaoCicloAvaliacao_ID.Value < 8)
                                            {
                                                AvaliacaoPDIColaborador avaliacaoPdi = new AvaliacaoPDIColaborador();

                                                AvaliacaoPDIColaboradorDAO avaliacaoPDIColaboradorDAO = new AvaliacaoPDIColaboradorDAO();

                                                avaliacaoPdi.DataCriacao = DateTime.Today;
                                                avaliacaoPdi.CicloAvaliacao_ID = model.CicloAvaliacaoSelecionadoID.Value;
                                                avaliacaoPdi.Colaborador_ID = usuarioCompetencia.ID;
                                                avaliacaoPdi.GestorRubi_ID = usuarioRubi.USU_LD1CAD;
                                                avaliacaoPdi.GestorRubiEmp_ID = usuarioRubi.USU_LD1EMP;
                                                avaliacaoPdi.StatusPDI_ID = (int)Enumeradores.StatusPDI.Criada;

                                                avaliacaoPdi.CargoRubiID = item.CargoRubiID;
                                                avaliacaoPdi.AreaRubiID = item.AreaRubiID;
                                                avaliacaoPdi.SetorRubiID = item.SetorRubiID;

                                                avaliacaoPDIColaboradorDAO.Incluir(avaliacaoPdi);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            return GestaoCompetenciasCargos(model.CicloAvaliacaoSelecionadoID, model.Pagina);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SobrescreverPeloUltimoCiclo(int? id)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(id.Value);

            if (ciclo != null)
            {
                //Pegar ultimoCiclo
                var ultimoCiclo = new CicloAvaliacaoDAO().ObterUltimoCiclo(id.Value, ciclo.DataInicioVigencia);

                //Pegar as associacoes do ultimoCiclo
                var associacoes = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(ultimoCiclo.ID);

                AssociacaoCargoCompetenciaDAO associacaoCargoCompetenciaDAO = new AssociacaoCargoCompetenciaDAO();

                if (associacoes != null)
                {
                    foreach (var item in associacoes)
                    {
                        var associacaoCargoCompetencia = new AssociacaoCargoCompetenciaDAO().Obter(id.Value, item.CargoRubiID, item.AreaRubiID, item.SetorRubiID.Value);
                        if (associacaoCargoCompetencia == null)
                        {
                            associacaoCargoCompetencia = new AssociacaoCargoCompetencia();
                            associacaoCargoCompetencia.CicloAvaliacao_ID = id.Value;
                            associacaoCargoCompetencia.AreaRubiID = item.AreaRubiID;
                            associacaoCargoCompetencia.AreaRubi = item.AreaRubi;
                            associacaoCargoCompetencia.CargoRubiID = item.CargoRubiID;
                            associacaoCargoCompetencia.CargoRubi = item.CargoRubi;
                            associacaoCargoCompetencia.SetorRubiID = item.SetorRubiID;
                            associacaoCargoCompetencia.SetorRubi = item.SetorRubi;
                            associacaoCargoCompetencia.AreaCompetenciaID = item.AreaCompetenciaID;
                            associacaoCargoCompetencia.CargoCompetenciaID = item.CargoCompetenciaID;
                            associacaoCargoCompetencia.SetorCompetenciaID = item.SetorCompetenciaID;
                            associacaoCargoCompetenciaDAO.Incluir(associacaoCargoCompetencia);
                        }
                        else
                        {
                            associacaoCargoCompetencia.AreaCompetenciaID = item.AreaCompetenciaID;
                            associacaoCargoCompetencia.CargoCompetenciaID = item.CargoCompetenciaID;
                            associacaoCargoCompetencia.SetorCompetenciaID = item.SetorCompetenciaID;
                            associacaoCargoCompetenciaDAO.Editar(associacaoCargoCompetencia);
                        }
                    }
                }
            }

            return GestaoCompetenciasCargos(id, 1);
        }

        private void CarregarAssociacoesCargoCompetencias(GestaoCompentenciasCargosViewModel model)
        {
            //model.AssociacoesCargosCompetencias =
            //        Mapper.Map<List<AssociacaoCargoCompetencia>,
            //                   List<ItemListaGestaoCompetenciasCargosViewModel>>
            //                        (new AssociacaoCargoCompetenciaDAO().
            //                            ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID.Value));

            model.AssociacoesCargosCompetencias = new List<ItemListaGestaoCompetenciasCargosViewModel>();

            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().
                                        ListarPorCicloAvaliacao(model.CicloAvaliacaoSelecionadoID.Value);

            if (associacoesCargosCompetencias != null)
            {
                foreach (var item in associacoesCargosCompetencias)
                {
                    model.AssociacoesCargosCompetencias.Add(new ItemListaGestaoCompetenciasCargosViewModel
                    {
                        AreaCompetencia = item.AreaCompetencia,
                        AreaCompetenciaID = item.AreaCompetenciaID,
                        AreaRubi = item.AreaRubi,
                        AreaRubiID = item.AreaRubiID,
                        CargoCompetencia = item.CargoCompetencia,
                        CargoCompetenciaID = item.CargoCompetenciaID,
                        CargoRubi = item.CargoRubi,
                        CargoRubiID = item.CargoRubiID,
                        CicloAvaliacaoID = item.CicloAvaliacao_ID,
                        ID = item.ID,
                        SetorCompetencia = item.SetorCompetencia,
                        SetorCompetenciaID = item.SetorCompetenciaID,
                        SetorRubi = item.SetorRubi,
                        SetorRubiID = item.SetorRubiID
                    });

                    if (item.CargoCompetenciaID.HasValue || item.SetorCompetenciaID.HasValue || item.AreaCompetenciaID.HasValue)
                    {
                        model.ExisteRelacaoSalva = true;
                    }
                }
            }

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
            var identidade = new Identidade();

            SelecaoCicloAvaliacaoViewModel model = new SelecaoCicloAvaliacaoViewModel();

            model.CiclosAvaliacao
                = Mapper.Map<List<CicloAvaliacao>,
                             List<SelectListItem>>(new CicloAvaliacaoDAO().ListarCiclosDisponiveis(identidade.CargoRubiID.Value, 
                                                                                                   identidade.AreaRubiID.Value, 
                                                                                                   identidade.SetorRubiID.Value,
                                                                                                   identidade.UsuarioRubiID.Value));

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

        [Authorize]
        [HttpGet]
        public ActionResult EnvioEmails(int cicloID, bool gestores)
        {
            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(cicloID);
            EnvioEmailsViewModel model = new EnvioEmailsViewModel();
            model.CicloAvaliacaoID = cicloID;

            if (cicloAvaliacao != null)
            {
                model.DescricaoCiclo = cicloAvaliacao.Descricao;
            }
            model.Gestores = gestores;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EnviarEmails(EnvioEmailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Usuario> gestores;
                if (model.Gestores)
                {
                    gestores = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(model.CicloAvaliacaoID.Value);
                }
                else
                {
                    gestores = new UsuarioDAO().ListarColaboradoresPorCicloAvaliacao(model.CicloAvaliacaoID.Value);
                }

                model.Email.ListaDeEmails = new List<string>();

                if (gestores != null && gestores.Any())
                {
                    foreach (var item in gestores)
                    {
                        model.Email.ListaDeEmails.Add(item.Email);
                    }

                    model.Email.Send();
                }

                return View("~/Views/CiclosAvaliacao/EmailEnviado.cshtml");
            }
            return View("~/Views/CiclosAvaliacao/EnvioEmails.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult VerEmail(EnvioEmailsViewModel model)
        {
            model.VerEmail = true;
            return View("~/Views/CiclosAvaliacao/EnvioEmails.cshtml", model);
        }
    }
}