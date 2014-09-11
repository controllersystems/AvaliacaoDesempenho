using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using System.Collections.Generic;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorViewModel
    {
        

















        private const string MENSAGEM_AVALIACAO_NAO_INICIADA = "Você ainda não iniciou a sua avaliação. Clique aqui para iniciar.";

        public string MensagemAvaliacaoNaoIniciada
        {
            get { return MENSAGEM_AVALIACAO_NAO_INICIADA; }
        }

        public SelecaoCicloAvaliacaoViewModel SelecaoCicloAvaliacaoViewModel { get; set; }

        public CicloAvaliacao CicloAvaliacao { get; set; }

        public int CicloAvaliacaoSelecionadoID { get; set; }

        public AvaliacaoColaborador AutoAvaliacao { get; set; }

        public int? AutoAvaliacaoSelecionadaID { get; set; }

        public AvaliacaoPDIColaborador AutoAvaliacaoPDI { get; set; }

        public ObjetivoColaborador ObjetivoSelecionado { get; set; }

        public List<AvaliacaoColaborador> AvaliacoesColaboradores { get; set; }

        public List<AvaliacaoPDIColaborador> AvaliacoesPDIColaboradores { get; set; }

        public bool IncluiMeta { get; set; }

        public ManterAvaliacaoColaboradorViewModel()
        {
            AutoAvaliacao = new AvaliacaoColaborador();

            AvaliacoesColaboradores = new List<AvaliacaoColaborador>();
            AvaliacoesPDIColaboradores = new List<AvaliacaoPDIColaborador>();

            SelecaoCicloAvaliacaoViewModel = new SelecaoCicloAvaliacaoViewModel();
        }
    }

}