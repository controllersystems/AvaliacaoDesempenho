using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
	public class ImpressaoEstruturadaViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int? StatusAvaliacaoPesquisadoID { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Cargo")]
        public string CargoPesquisado { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
        [Display(Name = "Colaborador")]
        public string ColaboradorPesquisado { get; set; }
        public List<ItemImpressaoEstruturadaViewModel> ListaImpressaoEstruturada { get; set; }
	}

    public class ItemImpressaoEstruturadaViewModel
    {
        public int ID { get; set; }
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public int ColaboradorID { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string StatusAvaliacao { get; set; }
    }
}