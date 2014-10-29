using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class AcompanhamentoEtapaViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public int EtapaSelecionada { get; set; }
        public List<ItemAcompanhamentoGeralViewModel> ListaAcompanhamentoEtapa { get; set; }
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
    }
}