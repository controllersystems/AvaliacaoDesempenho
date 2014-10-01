using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorCompetenciasGestorViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int StatusCicloAvaliacaoID { get; set; }

        public bool ProximaEtapa { get; set; }

        public IEnumerable<SelectListItem> ListaNivelAvaliacao { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasCorporativas { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasLideranca { get; set; }

        public List<ItemListaCompetenciasColaboradorGestor> ListaCompetenciasFuncionais { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaCompetenciasColaboradorGestor
    {
        public int? ID { get; set; }

        public int CompentenciaID { get; set; }

        public string Competencia { get; set; }

        public int NivelColaborador { get; set; }

        public int NivelRequerido { get; set; }

        [Display(Name = "Nível gestor")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public int? NivelGestor { get; set; }

        [Display(Name = "Comentário gestor")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(600, ErrorMessage = "O {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string ComentarioGestor { get; set; }
    }
}