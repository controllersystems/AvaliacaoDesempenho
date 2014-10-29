using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RatingFinalGestorViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemRatingFinalGestorViewModel> ListaRatingFinalGestor { get; set; }
        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
    }
    public class ItemRatingFinalGestorViewModel
    {
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string RatingFinal { get; set; }
        public string IndicacaoDePromocao { get; set; }
    }
}