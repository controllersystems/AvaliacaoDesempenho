using AvaliacaoDesempenho.Models.Avaliacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class ImpressaoEstruturadaIndividualViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public List<ObjetivoMetaResultadoAtingidoGestorViewModel> ListaObjetivosMetasResultadosatingidosGestorViewModel { get; set; }

        public List<OutrasContribuicoesGestorViewModel> ListaOutrasContribuicoesGestorViewModel { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasCorporativas { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasLideranca { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasFuncionais { get; set; }

        public IEnumerable<SelectListItem> ListaNivelAvaliacao { get; set; }

        public ItemListaPerformanceColaborador AvaliacaoPerformanceGerais { get; set; }

        public ItemListaRecomendacaoColaboradorRH ItemRecomendacaoColaborador { get; set; }

        public IEnumerable<SelectListItem> ListaRecomendacaoDeRating { get; set; }

        public ManterAvaliacaoPDIColaboradorViewModel PDI { get; set; }
    }
}