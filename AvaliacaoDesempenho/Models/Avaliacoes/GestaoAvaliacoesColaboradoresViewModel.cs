using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class GestaoAvaliacoesColaboradoresViewModel
    {
        public int CicloAvaliacaoSelecionadoID { get; set; }

        [Display(Name = "Pesquisar Avaliado")]
        public string NomeAvaliadoPesquisado { get; set; }

        public int? StatusAvaliacaoPesquisadoID { get; set; }

        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }

        [Display(Name = "Cargo")]
        public string CargoPesquisado { get; set; }

        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem> StatusAvaliacaoColaborador { get; set; }

        public List<ItemListaGestaoAvaliacoesViewModel> AvaliacoesColaboradores { get; set; }
    }

    public class SelecaoStatusAvaliacaoViewModel
    {
        public int StatusAvaliacaoSelecionadoID { get; set; }

        public List<SelectListItem> StatusAvaliacao { get; set; }
    }

    public class ItemListaGestaoAvaliacoesViewModel
    {
        public int ID { get; set; }

        public string NomeGestor { get; set; }

        public string Area { get; set; }

        public string UsuarioNome { get; set; }

        public string Cargo { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public string StatusAvaliacaoColaboradorNome { get; set; }
    }
}