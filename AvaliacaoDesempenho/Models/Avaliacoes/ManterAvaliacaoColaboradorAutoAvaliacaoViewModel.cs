using System.Collections.Generic;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorAutoAvaliacaoViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

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

        public string MetaColaboradorResultadoAtingidoColaboradorResultadoAtingido { get; set; }

        public bool AutoAvaliacao { get; set; }
    }

    public class OutrasContribuicoesViewModel
    {
        public int ID { get; set; }

        public string Contribuicao { get; set; }
    }

    public class AvaliacaoGestorContribuinte
    {
        public int ID { get; set; }
        public string Avaliacao { get; set; }
    }
}