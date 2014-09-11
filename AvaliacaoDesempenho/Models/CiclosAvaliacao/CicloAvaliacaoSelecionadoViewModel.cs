using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class CicloAvaliacaoSelecionadoViewModel
    {
        [Display(Name = "Ciclo de Avaliação")]
        public string Descricao { get; set; }
    }
}