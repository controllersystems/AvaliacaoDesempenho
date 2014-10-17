using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RatingFinalColaboradorViewModel
    {
        public List<ItemRatingFinalColaboradorViewModel> ListaRatingFinalColaborador { get; set; }
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