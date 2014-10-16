using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class ReuniaoCalibragemViewModel
    {
        public int AnoReferencia { get; set; }
        public List<ItemReuniaoCalibragemViewModel> ListaAcompanhamentoGeral { get; set; }
    }

    public class ItemReuniaoCalibragemViewModel
	{
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string Localidade { get; set; }
        public DateTime DataAdmissao { get; set; }
        public int TempoDeCasa { get; set; }
        public string RecomendacaoDeRating { get; set; }
        public string IndicacaoDePromocao { get; set; }
        public string PerformanceGeral { get; set; }
	} 
}