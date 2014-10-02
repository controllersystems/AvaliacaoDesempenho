using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorRecomendacaoRHViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? PapelID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int StatusCicloAvaliacaoID { get; set; }

        public IEnumerable<SelectListItem> ListaRecomendacaoDeRating { get; set; }

        public IEnumerable<SelectListItem> ListaSimNao { get; set; }

        public ItemListaRecomendacaoColaboradorRH ItemRecomendacaoColaborador { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaRecomendacaoColaboradorRH
    {
        public int? ID { get; set; }

        public int RecomendacaoDeRating { get; set; }

        public bool RecomendacaoDePromocao { get; set; }

        public string Justificativa { get; set; }

        [Display(Name = "Justificativa")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string JustificativaDaJustificativa { get; set; }

        [Display(Name = "Rating final pós calibragem")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public int? RatingFinalPosCalibragem { get; set; }

        [Display(Name = "Justificativa")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string JustificativaRatingFinalPosCalibragem { get; set; }

        [Display(Name = "Indicação promoção pós calibragem")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public bool? IndicacaoPromocaoPosCalibragem { get; set; }

        [Display(Name = "Justificativa")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string JustificativaIndicacaoPromocaoPosCalibragem { get; set; }
    }
}