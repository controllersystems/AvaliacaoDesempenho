using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class SelecaoCicloAvaliacaoViewModel
    {
        public int CicloAvaliacaoSelecionadoID { get; set; }

        public List<SelectListItem> CiclosAvaliacao { get; set; }
    }
}