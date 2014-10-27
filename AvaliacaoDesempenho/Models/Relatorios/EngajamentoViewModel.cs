using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class EngajamentoViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemEngajamentoViewModel> ListaEngajamento { get; set; }
    }
    public class ItemEngajamentoViewModel
    {
        
    }
}