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
                            CampoGap = (j.NivelGestor !=null && j.NivelRequerido != null) ? (j.NivelGestor.Value - j.NivelRequerido.Value) : 0
                        });
	}
                        
                    }
                }
            }
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
                            TempoDeCasa = (ano > 0 && mes != 0) ? ano + "ano(s) e " + mes + " mês(es)" : ((ano > 0 && mes == 0) ? ano + " ano(s)" : mes + " mês(es)"),
                            RecomendacaoDeRating = (recomendacao.RecomendacaoDeRating == 1) ? "Excepcional" : ((recomendacao.RecomendacaoDeRating == 2) ? "Excede as Expectativas" : ((recomendacao.RecomendacaoDeRating == 3) ? "Atende as Expectativas" : "Abaixo das Expectativas")),
                            IndicacaoDePromocao = (recomendacao.IndicacaoPromocaoPosCalibragem.HasValue) ? "Sim" : "Não",
                            PerformanceGeral = perfomance.AvaliacaoPerformance
                        });
                    }
                }
            }
            return View(model);
        }
    }
}
