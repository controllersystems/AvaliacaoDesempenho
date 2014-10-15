using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class AcompanhamentoGeralViewModel
    {
        public int AnoReferencia { get; set; }
        public List<ItemAcompanhamentoGeralViewModel> ListaAcompanhamentoGeral { get; set; }
    }

    public class ItemAcompanhamentoGeralViewModel
    {
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string StatusAvaliacao { get; set; }
        public DateTime UltimaAlteracao { get; set; }
    }
}