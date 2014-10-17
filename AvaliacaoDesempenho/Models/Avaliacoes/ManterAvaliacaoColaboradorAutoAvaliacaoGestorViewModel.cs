using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorAutoAvaliacaoGestorViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public DateTime? DataInicioAvaliacaoGestor { get; set; }

        public DateTime? DataTerminoAvaliacaoGestor { get; set; }

        public DateTime? DataInicioVigencia { get; set; }

        public DateTime? DataTerminoVigencia { get; set; }

        public int? SituacaoCicloAvaliaoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public bool IncluirMeta { get; set; }

        public bool IncluirContribuicao { get; set; }

        public ObjetivoMetaResultadoAtingidoGestorViewModel ObjetivoMetaResultadoAtingidoGestorCadastro { get; set; }

        public OutrasContribuicoesGestorViewModel OutrasContribuicoesGestorCadastro { get; set; }

        public List<ObjetivoMetaResultadoAtingidoGestorViewModel> ListaObjetivosMetasResultadosatingidosGestorViewModel { get; set; }

        public List<OutrasContribuicoesGestorViewModel> ListaOutrasContribuicoesGestorViewModel { get; set; }

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

    public class ObjetivoMetaResultadoAtingidoGestorViewModel
    {
        public int ID { get; set; }

        public string Objetivo { get; set; }

        public string MetaColaboradorMeta { get; set; }

        [Display(Name = "Resultado atingido")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido { get; set; }

        [Display(Name = "Avaliação do Gestor")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválido.")]
        [StringLength(400, ErrorMessage = "A {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string AvaliacaoGestor { get; set; }

        public bool AutoAvaliacao { get; set; }
    }

    public class OutrasContribuicoesGestorViewModel
    {
        public int ID { get; set; }

        public string Contribuicao { get; set; }

        [Display(Name = "Avaliação do Gestor")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválido.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        public string AvaliacaoGestor { get; set; }
    }
}