using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RatingFinalColaboradorViewModel
    {
        public int? CicloSelecionado { get; set; }
        public List<ItemRatingFinalColaboradorViewModel> ListaRatingFinalColaborador { get; set; }
        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
        [Display(Name = "Colaborador")]
        public string ColaboradorPesquisado { get; set; }
    }
    public class ItemRatingFinalColaboradorViewModel
    {
        public DateTime Data { get; set; }
        public string Nome { get; set; }
        public int Matricula { get; set; }
        public string NomeCiclo { get; set; }
        public string RatingFinalAposCalibragem { get; set; }
        public string Gestor { get; set; }
    }
}