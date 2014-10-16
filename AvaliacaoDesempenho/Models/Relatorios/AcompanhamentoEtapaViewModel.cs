using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class AcompanhamentoEtapaViewModel
    {
        public int AnoReferencia { get; set; }
        public int EtapaSelecionada { get; set; }
        public List<ItemAcompanhamentoGeralViewModel> ListaAcompanhamentoGeral { get; set; }
    }
}