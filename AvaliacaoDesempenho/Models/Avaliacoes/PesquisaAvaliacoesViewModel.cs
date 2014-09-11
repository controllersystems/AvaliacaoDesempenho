using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class PesquisaAvaliacoesViewModel
    {
        [Display(Name = "Pesquisar Avaliado")]
        public string NomeAvaliadoPesquisado { get; set; }

        public int StatusAvaliacaoPesquisadoID { get; set; }

        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }

        [Display(Name = "Cargo")]
        public string CargoPesquisado { get; set; }

        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
    }
}