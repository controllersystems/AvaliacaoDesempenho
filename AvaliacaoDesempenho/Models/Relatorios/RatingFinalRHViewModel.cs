using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RatingFinalRHViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemRatingFinalRHViewModel> ListaRatingFinalRH { get; set; }
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