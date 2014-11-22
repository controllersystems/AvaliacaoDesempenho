using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorRecomendacaoViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? PapelID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public DateTime? DataInicioAvaliacaoGestor { get; set; }

        public DateTime? DataTerminoAvaliacaoGestor { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int? StatusCicloAvaliacaoID { get; set; }

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

        [Display(Name = "Recomendação de Rating")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        //[Required(ErrorMessage = "A {0} é obrigatória.")]
        public int? RecomendacaoDeRating { get; set; }

        [Display(Name = "Recomendação de Promoção")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        //[Required(ErrorMessage = "A {0} é obrigatória.")]
        public Nullable<bool> RecomendacaoDePromocao { get; set; }

        [Display(Name = "Justificativa")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        //[Required(ErrorMessage = "A {0} é obrigatória.")]
        public string Justificativa { get; set; }

        [Display(Name = "Justificativa")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        //[Required(ErrorMessage = "A {0} é obrigatória.")]
        public string JustificativaDaJustificativa { get; set; }
    }
}