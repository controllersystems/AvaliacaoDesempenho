using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorObjetivosMetasViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public bool IncluirMeta { get; set; }

        public bool Aprovar { get; set; }

        public bool Aprovada { get; set; }

        [Display(Name = "Justificativa da reprovação")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(400, ErrorMessage = "A {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string JustificativaReprovacao { get; set; }

        public ObjetivoMetaViewModel ObjetivoMetaCadastro { get; set; }

        public List<ObjetivoMetaViewModel> ListaObjetivosMetasViewModel { get; set; }

        public bool AcessoGestor 
        { 
            get 
            { 
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value); 
            } 
        }
    }

    public class ObjetivoMetaViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Objetivo")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "A {0} deve ter o tamanho máximo de 400 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string Objetivo { get; set; }

        [Display(Name = "Meta")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(400, ErrorMessage = "A {0} deve ter o tamanho máximo de 400 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string MetaColaboradorMeta { get; set; }
    }
}