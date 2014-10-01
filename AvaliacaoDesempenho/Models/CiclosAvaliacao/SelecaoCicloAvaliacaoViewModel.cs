using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class SelecaoCicloAvaliacaoViewModel
    {
        [Display(Name = "Ciclo de Avaliação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [Required(ErrorMessage = "O {0} é obrigatória.")]
        public int CicloAvaliacaoSelecionadoID { get; set; }

        public List<SelectListItem> CiclosAvaliacao { get; set; }
    }
}