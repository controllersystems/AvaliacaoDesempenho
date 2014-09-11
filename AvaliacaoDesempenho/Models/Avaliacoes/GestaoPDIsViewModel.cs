using AvaliacaoDesempenho.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class GestaoPDIsViewModel : PesquisaAvaliacoesViewModel
    {
        [Display(Name = "Status")]
        public List<StatusPDI> StatusPDI { get; set; }

        public List<AvaliacaoPDIColaborador> AvaliacoesPDIsColaboradores { get; set; }

        public int StatusPDIPesquisadoID { get; set; }

        public GestaoPDIsViewModel()
        {
            StatusPDI = new List<StatusPDI>();
            AvaliacoesPDIsColaboradores = new List<AvaliacaoPDIColaborador>();
        }
    }
}