using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class RelatorioPDIViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemRelatorioPDIViewModel> ListaRelatorioPDI { get; set; }
    }
    public class ItemRelatorioPDIViewModel
    {
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string Idiomas { get; set; }
        public string Graduacao { get; set; }
        public string AcoesDesenvolvimento { get; set; }
    }
}