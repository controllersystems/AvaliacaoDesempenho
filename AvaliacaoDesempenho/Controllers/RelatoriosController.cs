using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvaliacaoDesempenho.Models.Relatorios;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Rubi.Integracoes;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Seguranca;
using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;

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
                            StatusAvaliacao = item.StatusAvaliacaoColaborador.Nome,
                            UltimaAlteracao = item.DataUltimaAlteracao
                        });
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarAcompanhamentoGeral(AcompanhamentoGeralViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaAcompanhamentoGeral = new List<ItemAcompanhamentoGeralViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.ColaboradorPesquisado != null)
                        {
                            if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.CargoPesquisado != null)
                        {
                            if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (!restringe)
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
            }

            return View("~/Views/Relatorios/AcompanhamentoGeral.cshtml", model);
        }

        public ActionResult AcompanhamentoEtapa(int? cicloSelecionado)
        {
            AcompanhamentoEtapaViewModel model = new AcompanhamentoEtapaViewModel();
            
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaAcompanhamentoEtapa = new List<ItemAcompanhamentoGeralViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        model.ListaAcompanhamentoEtapa.Add(new ItemAcompanhamentoGeralViewModel
                        {
                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            StatusAvaliacao = item.StatusAvaliacaoColaborador.Nome,
                            UltimaAlteracao = item.DataUltimaAlteracao
                        });
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarAcompanhamentoEtapa(AcompanhamentoEtapaViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaAcompanhamentoEtapa = new List<ItemAcompanhamentoGeralViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.ColaboradorPesquisado != null)
                        {
                            if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.CargoPesquisado != null)
                        {
                            if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (!restringe)
                        {
                            model.ListaAcompanhamentoEtapa.Add(new ItemAcompanhamentoGeralViewModel
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
            }

            return View("~/Views/Relatorios/AcompanhamentoEtapa.cshtml", model);
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
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);


            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaGapCompetencias = new List<ItemGapCompetenciasViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        var competenciaColaborador = new CompetenciaColaboradorDAO().Listar(item.ID);
                        foreach (var j in competenciaColaborador)
                        {
                            var competencias = new Integracoes.SistemaCompetencias.IntegracaoSistemaCompetencias().Obter(j.CompetenciaID);

                            model.ListaGapCompetencias.Add(new ItemGapCompetenciasViewModel
                                            {
                                                Diretoria = informacoesRubi.USU_CODDIR,
                                                Area = informacoesRubi.CODCCU,
                                                Gestor = informacoesRubi.LD1NOM,
                                                NomeColaborador = item.Usuario.Nome,
                                                Matricula = item.Usuario.UsuarioRubiID,
                                                Cargo = informacoesRubi.TITRED,
                                                TipoCompetencia = (competencias.id_tipo_comp == 1) ? "Funcionais" : (competencias.id_tipo_comp == 2) ? "Corporativa" : "Liderança",
                                                NomeCompetencia = (string.IsNullOrEmpty(competencias.descricao_comp)) ? competencias.titulo_comp : competencias.titulo_comp + " - " + competencias.descricao_comp,
                                                NivelRequirido = j.NivelRequerido,
                                                NivelAvaliadoGestor = j.NivelGestor,
                                                CampoGap = (j.NivelGestor != null && j.NivelRequerido != null) ? (j.NivelGestor.Value - j.NivelRequerido.Value) : 0
                                            });
                        }
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarGapCompetencias(GapCompetenciasViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaGapCompetencias = new List<ItemGapCompetenciasViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        var competenciaColaborador = new CompetenciaColaboradorDAO().Listar(item.ID);
                        foreach (var j in competenciaColaborador)
                        {
                            var competencias = new Integracoes.SistemaCompetencias.IntegracaoSistemaCompetencias().Obter(j.CompetenciaID);
                            bool restringe = false;
                            if (model.DiretoriaPesquisada != null)
                            {
                                if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.AreaPesquisada != null)
                            {
                                if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.GestorPesquisado != null)
                            {
                                if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.ColaboradorPesquisado != null)
                            {
                                if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.CargoPesquisado != null)
                            {
                                if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (!restringe)
                            {
                                model.ListaGapCompetencias.Add(new ItemGapCompetenciasViewModel
                                {
                                    Diretoria = informacoesRubi.USU_CODDIR,
                                    Area = informacoesRubi.CODCCU,
                                    Gestor = informacoesRubi.LD1NOM,
                                    NomeColaborador = item.Usuario.Nome,
                                    Matricula = item.Usuario.UsuarioRubiID,
                                    Cargo = informacoesRubi.TITRED,
                                    TipoCompetencia = (competencias.id_tipo_comp == 1) ? "Funcionais" : (competencias.id_tipo_comp == 2) ? "Corporativa" : "Liderança",
                                    NomeCompetencia = competencias.descricao_comp,
                                    NivelRequirido = j.NivelRequerido,
                                    NivelAvaliadoGestor = j.NivelGestor,
                                    CampoGap = (j.NivelGestor != null && j.NivelRequerido != null) ? (j.NivelGestor.Value - j.NivelRequerido.Value) : 0
                                });
                            }
                        }
                    }
                }
            }

            return View("~/Views/Relatorios/GapCompetencias.cshtml", model);
        }
        public ActionResult RatingFinalColaborador(int? cicloSelecionado)
        {
            RatingFinalColaboradorViewModel model = new RatingFinalColaboradorViewModel();
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRatingFinalColaborador = new List<ItemRatingFinalColaboradorViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    if (informacoesRubi != null)
                    {
                        model.ListaRatingFinalColaborador.Add(new ItemRatingFinalColaboradorViewModel
                        {
                            //Data =
                            Nome = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            NomeCiclo = ciclo.Descricao,
                            RatingFinalAposCalibragem = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")): " "),
                            Gestor = informacoesRubi.LD1NOM 
                        });
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarRatingFinalColaborador(RatingFinalColaboradorViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRatingFinalColaborador = new List<ItemRatingFinalColaboradorViewModel>();
                foreach (var item in avaliacoes)
                {
                    
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.ColaboradorPesquisado != null)
                        {
                            if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (!restringe)
                        {
                            model.ListaRatingFinalColaborador.Add(new ItemRatingFinalColaboradorViewModel
                            {
                                //Data =
                                Nome = item.Usuario.Nome,
                                Matricula = item.Usuario.UsuarioRubiID,
                                NomeCiclo = ciclo.Descricao,
                                RatingFinalAposCalibragem = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")) : " "),
                                Gestor = informacoesRubi.LD1NOM
                            });
                        }
                    }
                }
            }

            return View("~/Views/Relatorios/RatingFinalColaborador.cshtml", model);
        }
        public ActionResult RatingFinalGestor(int? cicloSelecionado)
        {
            RatingFinalGestorViewModel model = new RatingFinalGestorViewModel();

            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRatingFinalGestor = new List<ItemRatingFinalGestorViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    if (informacoesRubi != null)
                    {
                        model.ListaRatingFinalGestor.Add(new ItemRatingFinalGestorViewModel
                        {
                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")):" "),
                            IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.RecomendacaoDePromocao.HasValue) ? (recomendacao.RecomendacaoDePromocao.Value ? "Sim" : "Não") : "") : " ")
                        });
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarRatingFinalGestor(RatingFinalGestorViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRatingFinalGestor = new List<ItemRatingFinalGestorViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }
                        if (!restringe)
                        {
                            model.ListaRatingFinalGestor.Add(new ItemRatingFinalGestorViewModel
                            {
                                Diretoria = informacoesRubi.USU_CODDIR,
                                Area = informacoesRubi.CODCCU,
                                Gestor = informacoesRubi.LD1NOM,
                                NomeColaborador = item.Usuario.Nome,
                                Matricula = item.Usuario.UsuarioRubiID,
                                Cargo = informacoesRubi.TITRED,
                                RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")):" "),
                                IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.RecomendacaoDePromocao.HasValue) ? (recomendacao.RecomendacaoDePromocao.Value ? "Sim" : "Não") : "") : " "),
                            });
                        }
                    }
                }
            }

            return View("~/Views/Relatorios/RatingFinalGestor.cshtml", model);
        }
        public ActionResult RatingFinalRH(int? cicloSelecionado)
        {
            RatingFinalRHViewModel model = new RatingFinalRHViewModel();
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);
            if (avaliacoes != null)
            {
                model.ListaRatingFinalRH = new List<ItemRatingFinalRHViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        DateTime dataAtual = DateTime.Now;
                        TimeSpan ts = dataAtual.Subtract(informacoesRubi.DATADM.Value);
                        double tempo = ts.TotalDays;
                        int ano = Convert.ToInt32(tempo) / 365;
                        double resto = tempo % 365;
                        int mes = Convert.ToInt32(resto) / 30;
                        var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                        var perfomance = new PerformanceColaboradorDAO().Obter(item.ID);
                        model.ListaRatingFinalRH.Add(new ItemRatingFinalRHViewModel
                        {
                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            Localidade = informacoesRubi.NOMLOC,
                            DataAdmissao = (informacoesRubi.DATADM.HasValue) ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????",
                            TempoCasa = (ano > 0 && mes != 0) ? ano + " ano(s) e " + mes + " mes(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mes(es)"),
                            RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")):" "),
                            IndicacaoFinalPromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? (recomendacao.IndicacaoPromocaoPosCalibragem.Value ? "Sim" : "Não") : "") : " ")
                        });
                    }
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

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarRatingFinalRH(RatingFinalRHViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRatingFinalRH = new List<ItemRatingFinalRHViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.ColaboradorPesquisado != null)
                        {
                            if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.CargoPesquisado != null)
                        {
                            if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.LocalidadePesquisado != null)
                        {
                            if (!informacoesRubi.NOMLOC.ToUpper().Contains(model.LocalidadePesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);

                        if (recomendacao != null)
                        {
                            if (model.IndicacaoDePromocaoPesquisado.HasValue)
                            {
                                if (recomendacao.IndicacaoPromocaoPosCalibragem != model.IndicacaoDePromocaoPesquisado.Value)
                                {
                                    restringe = true;
                                }
                            }

                            if (model.RecomendacaoDeRatingPesquisado.HasValue)
                            {
                                if (recomendacao.RecomendacaoDeRating != model.RecomendacaoDeRatingPesquisado.Value)
                                {
                                    restringe = true;
                                }
                            }
                        }

                        if (!restringe)
                        {
                            DateTime dataAtual = DateTime.Now;
                            TimeSpan ts = dataAtual.Subtract(informacoesRubi.DATADM.Value);
                            double tempo = ts.TotalDays;
                            int ano = Convert.ToInt32(tempo) / 365;
                            double resto = tempo % 365;
                            int mes = Convert.ToInt32(resto) / 30;
                            var perfomance = new PerformanceColaboradorDAO().Obter(item.ID);
                            model.ListaRatingFinalRH.Add(new ItemRatingFinalRHViewModel
                            {
                                Diretoria = informacoesRubi.USU_CODDIR,
                                Area = informacoesRubi.CODCCU,
                                Gestor = informacoesRubi.LD1NOM,
                                NomeColaborador = item.Usuario.Nome,
                                Matricula = item.Usuario.UsuarioRubiID,
                                Cargo = informacoesRubi.TITRED,
                                Localidade = informacoesRubi.NOMLOC,
                                DataAdmissao = (informacoesRubi.DATADM.HasValue) ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????",
                                TempoCasa = (ano > 0 && mes != 0) ? ano + " ano(s) e " + mes + " mes(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mes(es)"),
                                RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")) : " "),
                                IndicacaoFinalPromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? (recomendacao.IndicacaoPromocaoPosCalibragem.Value ? "Sim" : "Não") : "") : " ")
                            });
                        }
                    }
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

            return View("~/Views/Relatorios/RatingFinalRH.cshtml", model);
        }

        public ActionResult RelatorioPDI(int? cicloSelecionado)
        {
            RelatorioPDIViewModel model = new RelatorioPDIViewModel();
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoPDIColaboradorDAO().Listar(cicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRelatorioPDI = new List<ItemRelatorioPDIViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    if (informacoesRubi != null)
                    {
                        var acoesDesenvolvimento = new DesenvolvimentoCompetenciaDAO().Listar(item.ID);
                        foreach (var j in acoesDesenvolvimento)
                        {
                           model.ListaRelatorioPDI.Add(new ItemRelatorioPDIViewModel
                        {
                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            Idiomas = item.Idiomas, 
                            Graduacao = item.Graduacao, 
                            AcoesDesenvolvimento = j.AcaoDesenvolvimento 
                        });  
                        }
                       
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarRelatorioPDI(RelatorioPDIViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoPDIColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaRelatorioPDI = new List<ItemRelatorioPDIViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);
                    var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                    if (informacoesRubi != null)
                    {
                        var acoesDesenvolvimento = new DesenvolvimentoCompetenciaDAO().Listar(item.ID);
                        foreach (var j in acoesDesenvolvimento)
                        {
                            bool restringe = false;
                            if (model.DiretoriaPesquisada != null)
                            {
                                if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.AreaPesquisada != null)
                            {
                                if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.GestorPesquisado != null)
                            {
                                if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.ColaboradorPesquisado != null)
                            {
                                if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (model.CargoPesquisado != null)
                            {
                                if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                                {
                                    restringe = true;
                                }
                            }

                            if (!restringe)
                            {
                                model.ListaRelatorioPDI.Add(new ItemRelatorioPDIViewModel
                                {
                                    Diretoria = informacoesRubi.USU_CODDIR,
                                    Area = informacoesRubi.CODCCU,
                                    Gestor = informacoesRubi.LD1NOM,
                                    NomeColaborador = item.Usuario.Nome,
                                    Matricula = item.Usuario.UsuarioRubiID,
                                    Cargo = informacoesRubi.TITRED,
                                    Idiomas = item.Idiomas,
                                    Graduacao = item.Graduacao,
                                    AcoesDesenvolvimento = j.AcaoDesenvolvimento
                                });    
                            }
                        }
                    }
                }
            }

            return View("~/Views/Relatorios/RelatorioPDI.cshtml", model);
        }

        public ActionResult ReuniaoCalibragem(int? cicloSelecionado)
        {
            ReuniaoCalibragemViewModel model = new ReuniaoCalibragemViewModel();
            var ciclo = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            model.CicloSelecionado = cicloSelecionado;
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);
            if (avaliacoes != null)
            {
                model.ListaReuniaoCalibragem = new List<ItemReuniaoCalibragemViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        DateTime dataAtual = DateTime.Now;
                        TimeSpan ts = dataAtual.Subtract(informacoesRubi.DATADM.Value);
                        double tempo = ts.TotalDays;
                        int ano = Convert.ToInt32(tempo) / 365;
                        double resto = tempo % 365;
                        int mes = Convert.ToInt32(resto) / 30;
                        var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);
                        var perfomance = new PerformanceColaboradorDAO().Obter(item.ID);
                        model.ListaReuniaoCalibragem.Add(new ItemReuniaoCalibragemViewModel
                        {

                            Diretoria = informacoesRubi.USU_CODDIR,
                            Area = informacoesRubi.CODCCU,
                            Gestor = informacoesRubi.LD1NOM,
                            NomeColaborador = item.Usuario.Nome,
                            Matricula = item.Usuario.UsuarioRubiID,
                            Cargo = informacoesRubi.TITRED,
                            Localidade = informacoesRubi.NOMLOC,
                            DataAdmissao = (informacoesRubi.DATADM.HasValue) ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????",
                            TempoDeCasa = (ano > 0 && mes != 0) ? ano + " ano(s) e " + mes + " mes(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mes(es)"),
                            RecomendacaoDeRating = (recomendacao != null ? (recomendacao.RecomendacaoDeRating == 1) ? "Excepcional" : ((recomendacao.RecomendacaoDeRating == 2) ? "Excede as Expectativas" : ((recomendacao.RecomendacaoDeRating == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")) : " "),
                            IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.RecomendacaoDePromocao.HasValue) ? (recomendacao.RecomendacaoDePromocao.Value ? "Sim" : "Não") : "") : " "),
                            PerformanceGeral = (perfomance != null ? perfomance.AvaliacaoPerformance : " ")
                        });
                    }
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

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PesquisarReuniaoCalibragem(ReuniaoCalibragemViewModel model)
        {
            var ciclo = new CicloAvaliacaoDAO().Obter(model.CicloSelecionado.Value);

            if (ciclo != null)
            {
                model.AnoReferencia = ciclo.DataFimVigencia.Year;
            }

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            if (avaliacoes != null)
            {
                model.ListaReuniaoCalibragem = new List<ItemReuniaoCalibragemViewModel>();
                foreach (var item in avaliacoes)
                {
                    var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(item.Usuario.CodigoEmpresaRubiUD, item.Usuario.UsuarioRubiID);

                    if (informacoesRubi != null)
                    {
                        bool restringe = false;
                        if (model.DiretoriaPesquisada != null)
                        {
                            if (!informacoesRubi.USU_CODDIR.ToUpper().Contains(model.DiretoriaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.AreaPesquisada != null)
                        {
                            if (!informacoesRubi.CODCCU.ToUpper().Contains(model.AreaPesquisada.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.GestorPesquisado != null)
                        {
                            if (!informacoesRubi.LD1NOM.ToUpper().Contains(model.GestorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.ColaboradorPesquisado != null)
                        {
                            if (!item.Usuario.Nome.ToUpper().Contains(model.ColaboradorPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.CargoPesquisado != null)
                        {
                            if (!informacoesRubi.TITRED.ToUpper().Contains(model.CargoPesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        if (model.LocalidadePesquisado != null)
                        {
                            if (!informacoesRubi.NOMLOC.ToUpper().Contains(model.LocalidadePesquisado.ToUpper()))
                            {
                                restringe = true;
                            }
                        }

                        var recomendacao = new RecomendacaoColaboradorDAO().Obter(item.ID);

                        if (recomendacao != null)
                        {
                            if (model.IndicacaoDePromocaoPesquisado.HasValue)
                            {
                                if (recomendacao.RecomendacaoDePromocao != model.IndicacaoDePromocaoPesquisado.Value)
                                {
                                    restringe = true;
                                }
                            }

                            if (model.RecomendacaoDeRatingPesquisado.HasValue)
                            {
                                if (recomendacao.RecomendacaoDeRating != model.RecomendacaoDeRatingPesquisado.Value)
                                {
                                    restringe = true;
                                }
                            }
                        }

                        if (!restringe)
                        {
                            DateTime dataAtual = DateTime.Now;
                            TimeSpan ts = dataAtual.Subtract(informacoesRubi.DATADM.Value);
                            double tempo = ts.TotalDays;
                            int ano = Convert.ToInt32(tempo) / 365;
                            double resto = tempo % 365;
                            int mes = Convert.ToInt32(resto) / 30;
                            var perfomance = new PerformanceColaboradorDAO().Obter(item.ID);
                            model.ListaReuniaoCalibragem.Add(new ItemReuniaoCalibragemViewModel
                            {
                                Diretoria = informacoesRubi.USU_CODDIR,
                                Area = informacoesRubi.CODCCU,
                                Gestor = informacoesRubi.LD1NOM,
                                NomeColaborador = item.Usuario.Nome,
                                Matricula = item.Usuario.UsuarioRubiID,
                                Cargo = informacoesRubi.TITRED,
                                Localidade = informacoesRubi.NOMLOC,
                                DataAdmissao = (informacoesRubi.DATADM.HasValue) ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????",
                                TempoDeCasa = (ano > 0 && mes != 0) ? ano + " ano(s) e " + mes + " mes(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mes(es)"),
                                RecomendacaoDeRating = (recomendacao != null ? (recomendacao.RecomendacaoDeRating == 1) ? "Excepcional" : ((recomendacao.RecomendacaoDeRating == 2) ? "Excede as Expectativas" : ((recomendacao.RecomendacaoDeRating == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")) : " "),
                                IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.RecomendacaoDePromocao.HasValue) ? (recomendacao.RecomendacaoDePromocao.Value ? "Sim" : "Não") : "") : " "),
                                PerformanceGeral = (perfomance != null ? perfomance.AvaliacaoPerformance : " ")
                            });
                        }
                    }
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

            return View("~/Views/Relatorios/ReuniaoCalibragem.cshtml", model);
        }

        public ActionResult ImpressaoEstruturada(int? cicloSelecionado)
        {
            ImpressaoEstruturadaViewModel model = new ImpressaoEstruturadaViewModel();

            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(cicloSelecionado.Value);

            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(cicloSelecionado.Value);
            var gestoresCicloAvaliacao = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(cicloSelecionado.Value);

            if (avaliacoes != null
               && associacoesCargosCompetencias != null
                && gestoresCicloAvaliacao != null)
            {
                model.ListaImpressaoEstruturada =
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
                     select new ItemImpressaoEstruturadaViewModel()
                     {
                         ID = avaliacao.ID,
                         Gestor = (avaliacao.Usuario == null) ? "" : (gestoresCicloAvaliacao.Exists(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID) ? gestoresCicloAvaliacao.Find(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID).Nome : ""),
                         NomeColaborador = avaliacao.Usuario.Nome,
                         Cargo = associacao.CargoRubi,
                         Area = associacao.AreaRubi,
                         StatusAvaliacao = avaliacao.StatusAvaliacaoColaborador.Nome,
                         ColaboradorID = avaliacao.Usuario.ID
                     }).ToList();
            }

            model.CicloSelecionado = cicloSelecionado;

            return View(model);
        }

        [HttpPost]
        public ActionResult PesquisarImpressaoEstruturada(ImpressaoEstruturadaViewModel model)
        {
            var avaliacoes = new AvaliacaoColaboradorDAO().Listar(model.CicloSelecionado.Value);

            var associacoesCargosCompetencias = new AssociacaoCargoCompetenciaDAO().ListarPorCicloAvaliacao(model.CicloSelecionado.Value);
            var gestoresCicloAvaliacao = new UsuarioDAO().ListarGestoresPorCicloAvaliacao(model.CicloSelecionado.Value);

            if (avaliacoes != null
               && associacoesCargosCompetencias != null
                && gestoresCicloAvaliacao != null)
            {
                model.ListaImpressaoEstruturada =
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
                                     .Contains(!string.IsNullOrEmpty(model.ColaboradorPesquisado)
                                               && !string.IsNullOrWhiteSpace(model.ColaboradorPesquisado)
                                                     ? model.ColaboradorPesquisado.Trim().ToUpper()
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
                     select new ItemImpressaoEstruturadaViewModel()
                     {
                         ID = avaliacao.ID,
                         Gestor = (avaliacao.Usuario == null) ? "" : (gestoresCicloAvaliacao.Exists(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID) ? gestoresCicloAvaliacao.Find(x => x.CodigoEmpresaRubiUD == avaliacao.GestorRubiEmp_ID && x.UsuarioRubiID == avaliacao.GestorRubi_ID).Nome : ""),
                         NomeColaborador = avaliacao.Usuario.Nome,
                         Cargo = associacao.CargoRubi,
                         Area = associacao.AreaRubi,
                         StatusAvaliacao = avaliacao.StatusAvaliacaoColaborador.Nome,
                         ColaboradorID = avaliacao.Usuario.ID
                     }).ToList();

                model.ListaImpressaoEstruturada = model.ListaImpressaoEstruturada
                                                     .Where(x => x.Gestor.Contains(!string.IsNullOrEmpty(model.GestorPesquisado)
                                                                                       && !string.IsNullOrWhiteSpace(model.GestorPesquisado)
                                                                                       ? model.GestorPesquisado.Trim().ToUpper()
                                                                                       : x.Gestor.ToUpper())).ToList();
            }

            return View("~/Views/Relatorios/ImpressaoEstruturada.cshtml", model);
        }

        public ActionResult ImpressaoEstruturadaIndividual(int? cicloSelecionado, int? colaboradorID)
        {
            var identidade = new Identidade();

            int usuarioID;

            if (colaboradorID.HasValue)
                usuarioID = colaboradorID.Value;
            else
                usuarioID = identidade.UsuarioID;

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

            ImpressaoEstruturadaIndividualViewModel model=new ImpressaoEstruturadaIndividualViewModel();

            var cicloAvaliacao = new CicloAvaliacaoDAO().Obter(cicloSelecionado.Value);

            if (cicloAvaliacao != null)
            {
                var avaliacaoColaborador =
                    new AvaliacaoColaboradorDAO().Obter(cicloSelecionado.Value, usuarioID);

                if (avaliacaoColaborador != null)
                {
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

                    var associacaoCargoCompentencia = new AssociacaoCargoCompetenciaDAO().Obter(cicloSelecionado.Value, avaliacaoColaborador.CargoRubiID.Value, avaliacaoColaborador.AreaRubiID.Value, avaliacaoColaborador.SetorRubiID.Value);

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

                    //listaAval.Add(new SelectListItem() { Text = "N/A", Value = "-1" });

                    for (int i = 0; i < 5; i++)
                    {
                        listaAval.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }

                    model.ListaNivelAvaliacao = listaAval;

                    var performanceColaborador = new PerformanceColaboradorDAO().Obter(avaliacaoColaborador.ID);

                    model.AvaliacaoPerformanceGerais = new ItemListaPerformanceColaborador();

                    if (performanceColaborador != null)
                    {
                        model.AvaliacaoPerformanceGerais.ID = performanceColaborador.ID;
                        model.AvaliacaoPerformanceGerais.AvaliacaoPerformanceGeral = performanceColaborador.AvaliacaoPerformance;
                    }

                    var recomendacaoColaborador = new RecomendacaoColaboradorDAO().Obter(avaliacaoColaborador.ID);

                    model.ItemRecomendacaoColaborador = new ItemListaRecomendacaoColaboradorRH();

                    if (recomendacaoColaborador != null)
                    {
                        model.ItemRecomendacaoColaborador.RecomendacaoDeRating = recomendacaoColaborador.RecomendacaoDeRating;
                        model.ItemRecomendacaoColaborador.RecomendacaoDePromocao = recomendacaoColaborador.RecomendacaoDePromocao;
                        model.ItemRecomendacaoColaborador.Justificativa = recomendacaoColaborador.Justificativa;
                        model.ItemRecomendacaoColaborador.JustificativaDaJustificativa = recomendacaoColaborador.JustificativaDaJustificativa;

                        model.ItemRecomendacaoColaborador.RatingFinalPosCalibragem = recomendacaoColaborador.RatingFinalPosCalibragem;
                        model.ItemRecomendacaoColaborador.IndicacaoPromocaoPosCalibragem = recomendacaoColaborador.IndicacaoPromocaoPosCalibragem;
                        model.ItemRecomendacaoColaborador.JustificativaRatingFinalPosCalibragem = recomendacaoColaborador.JustificativaRatingFinalPosCalibragem;
                        model.ItemRecomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem = recomendacaoColaborador.JustificativaIndicacaoPromocaoPosCalibragem;
                    }

                    listaAval = new List<SelectListItem>();

                    listaAval.Add(new SelectListItem() { Text = "Excepcional", Value = "1" });
                    listaAval.Add(new SelectListItem() { Text = "Excede as Expectativas", Value = "2" });
                    listaAval.Add(new SelectListItem() { Text = "Atende as Expectativas", Value = "3" });
                    listaAval.Add(new SelectListItem() { Text = "Abaixo das Expectativas", Value = "4" });

                    model.ListaRecomendacaoDeRating = listaAval;
                }

                var avaliacaoPDIColaborador =
                new AvaliacaoPDIColaboradorDAO().Obter(cicloSelecionado.Value, usuarioID);

                if (avaliacaoPDIColaborador != null)
                {
                    Mapper.CreateMap<DesenvolvimentoCompetencia, ItemListaDesenvolvimentoCompetenciaViewModel>();

                    model.PDI = new ManterAvaliacaoPDIColaboradorViewModel();

                    model.PDI.ListaDesenvolvimentoCompetenciaViewModel =
                        Mapper.Map<List<DesenvolvimentoCompetencia>,
                                   List<ItemListaDesenvolvimentoCompetenciaViewModel>>
                                        (new DesenvolvimentoCompetenciaDAO().Listar(avaliacaoPDIColaborador.ID));

                    model.PDI.AvaliacaoPDIColaboradorID = avaliacaoPDIColaborador.ID;

                    model.PDI.Idiomas = avaliacaoPDIColaborador.Idiomas;

                    model.PDI.Graduacao = avaliacaoPDIColaborador.Graduacao;

                    model.PDI.PontosFortes = avaliacaoPDIColaborador.PontosFortes;

                    model.PDI.PontosDesenvolvimento = avaliacaoPDIColaborador.PontosDesenvolvimento;

                    model.PDI.ComentariosColaborador = avaliacaoPDIColaborador.ComentariosColaborador;

                    model.PDI.ComentariosGestor = avaliacaoPDIColaborador.ComentariosGestor;
                }
            }

            return View(model);
        }
    }
}
