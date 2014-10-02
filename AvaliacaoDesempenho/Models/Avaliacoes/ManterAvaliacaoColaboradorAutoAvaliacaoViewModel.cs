using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorAutoAvaliacaoViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public bool IncluirMeta { get; set; }

        public bool IncluirContribuicao { get; set; }

        public ObjetivoMetaResultadoAtingidoViewModel ObjetivoMetaResultadoAtingidoCadastro { get; set; }

        public OutrasContribuicoesViewModel OutrasContribuicoesCadastro { get; set; }

        public List<ObjetivoMetaResultadoAtingidoViewModel> ListaObjetivosMetasResultadosatingidosViewModel { get; set; }

        public List<OutrasContribuicoesViewModel> ListaOutrasContribuicoesViewModel { get; set; }

        public List<AvaliacaoGestorContribuinte> ListaAvaliacaoGestorMetas { get; set; }

        public List<AvaliacaoGestorContribuinte> ListaAvaliacaoGestorOutrasContribuicoes { get; set; }

        public bool ProximaEtapa { get; set; } 


        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ObjetivoMetaResultadoAtingidoViewModel
    {
        public int ID { get; set; }

        public string Objetivo { get; set; }

        public string MetaColaboradorMeta { get; set; }

        [Display(Name = "Resultado atingido")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido { get; set; }

        public bool AutoAvaliacao { get; set; }
    }

    public class OutrasContribuicoesViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Contribuição")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválido.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string Contribuicao { get; set; }
    }

    public class AvaliacaoGestorContribuinte
    {
        public int ID { get; set; }
        public string Avaliacao { get; set; }
    }
}