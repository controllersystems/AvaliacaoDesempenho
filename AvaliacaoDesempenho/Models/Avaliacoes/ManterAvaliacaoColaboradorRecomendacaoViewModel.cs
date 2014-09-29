using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorRecomendacaoViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? PapelID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int StatusCicloAvaliacaoID { get; set; }

        public IEnumerable<SelectListItem> ListaRecomendacaoDeRating { get; set; }

        public IEnumerable<SelectListItem> ListaSimNao { get; set; }

        public ItemListaRecomendacaoColaborador ItemRecomendacaoColaborador { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaRecomendacaoColaborador
    {
        public int? ID { get; set; }

        public int RecomendacaoDeRating { get; set; }

        public bool RecomendacaoDePromocao { get; set; }

        public string Justificativa { get; set; }

        public string JustificativaDaJustificativa { get; set; }

        public int? RatingFinalPosCalibragem { get; set; }

        public string JustificativaRatingFinalPosCalibragem { get; set; }

        public bool? IndicacaoPromocaoPosCalibragem { get; set; }

        public string JustificativaIndicacaoPromocaoPosCalibragem { get; set; }
    }
}