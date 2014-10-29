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
                            StatusAvaliacao = item.StatusAvaliacaoColaborador.Nome
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
                                                NomeCompetencia = competencias.descricao_comp,
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
                            IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? "Sim":"Não"):" ")
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
                                IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? "Sim" : "Não"):" ")
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
                            RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")):" ")
                        });
                    }
                }
            }
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

                        if (!restringe)
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
                                RatingFinal = (recomendacao != null ? (recomendacao.RatingFinalPosCalibragem == 1) ? "Excepcional" : ((recomendacao.RatingFinalPosCalibragem == 2) ? "Excede as Expectativas" : ((recomendacao.RatingFinalPosCalibragem == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")) : " ")
                            });
                        }
                    }
                }
            }

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
                        int ano = Convert.ToInt32(tempo)/365; 
                        double resto = tempo % 365;
                        int mes = Convert.ToInt32(resto)/30;
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
                            Localidade =  informacoesRubi.NOMLOC, 
                            DataAdmissao = (informacoesRubi.DATADM.HasValue) ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????",
                            TempoDeCasa = (ano > 0 && mes != 0) ? ano + " ano(s) e " + mes + " mes(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mes(es)"),
                            RecomendacaoDeRating = (recomendacao != null ? (recomendacao.RecomendacaoDeRating == 1) ? "Excepcional" : ((recomendacao.RecomendacaoDeRating == 2) ? "Excede as Expectativas" : ((recomendacao.RecomendacaoDeRating == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")):" "),
                            IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? "Sim" : "Não"):" "),
                            PerformanceGeral = (perfomance != null ? perfomance.AvaliacaoPerformance : " ")
                        });
                    }
                }
            }
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

                        if (!restringe)
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
                                IndicacaoDePromocao = (recomendacao != null ? ((recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? "Sim" : "Não") : " "),
                                PerformanceGeral = (perfomance != null ? perfomance.AvaliacaoPerformance : " ")
                            });
                        }
                    }
                }
            }

            return View("~/Views/Relatorios/ReuniaoCalibragem.cshtml", model);
        }
    }
}
