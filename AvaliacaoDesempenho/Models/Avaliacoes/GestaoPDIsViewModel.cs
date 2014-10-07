using AvaliacaoDesempenho.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class GestaoPDIsViewModel : PesquisaAvaliacoesViewModel
    {
        public int CicloAvaliacaoSelecionadoID { get; set; }

        public string CicloAvaliacaoDescricao { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem> StatusPDI { get; set; }

        public List<ItemListaGestaoAvaliacoesPDIViewModel> AvaliacoesPDIsColaboradores { get; set; }

        public int? StatusPDIPesquisadoID { get; set; }

        public GestaoPDIsViewModel()
        {
            //StatusPDI = new List<StatusPDI>();
            AvaliacoesPDIsColaboradores = new List<ItemListaGestaoAvaliacoesPDIViewModel>();
        }
    }

    public class ItemListaGestaoAvaliacoesPDIViewModel
    {
        public int ID { get; set; }

        public string NomeGestor { get; set; }

        public string Area { get; set; }

        public string UsuarioNome { get; set; }

        public int ColaboradorID { get; set; }

        public string Cargo { get; set; }

        public int StatusPDIID { get; set; }

        public string StatusPDINome { get; set; }
    }
}