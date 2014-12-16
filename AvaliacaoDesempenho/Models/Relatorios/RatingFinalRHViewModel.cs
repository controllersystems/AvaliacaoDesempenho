using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RatingFinalRHViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemRatingFinalRHViewModel> ListaRatingFinalRH { get; set; }
        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
        [Display(Name = "Colaborador")]
        public string ColaboradorPesquisado { get; set; }
        [Display(Name = "Cargo")]
        public string CargoPesquisado { get; set; }
        [Display(Name = "Localidade")]
        public string LocalidadePesquisado { get; set; }
        [Display(Name = "Recomendação de Rating")]
        public int? RecomendacaoDeRatingPesquisado { get; set; }
        [Display(Name = "Indicação de Promoção")]
        public bool? IndicacaoDePromocaoPesquisado { get; set; }
        public IEnumerable<SelectListItem> ListaSimNao { get; set; }
        public IEnumerable<SelectListItem> ListaRecomendacaoDeRating { get; set; }
    }
    public class ItemRatingFinalRHViewModel
    {
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string Localidade { get; set; }
        public string DataAdmissao { get; set; }
        public string TempoCasa { get; set; }
        public string RatingFinal { get; set; }
        public string IndicacaoFinalPromocao { get; set; }
    }
}