using System.Collections.Generic;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class GestaoAvaliacaoColaboradorViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? SituacaoCicloAvaliacaoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public string DataCriacao { get; set; }

        public string TituloOrientacaoObjetivosMetas { get; set; }

        public string TextoOrientacaoObjetivosMetas { get; set; }

        public string TituloOrientacaoAutoAvaliacao { get; set; }

        public string TextoOrientacaoAutoAvaliacao { get; set; }

        public string TituloOrientacaoAvaliacaoGestor { get; set; }

        public string TextoOrientacaoAvaliacaoGestor { get; set; }

        public string StatusAvaliacaoColaboradorNome { get; set; }

        public bool ParticipaDaAvaliacao { get; set; }

        public string OrientacaoCompetencia { get; set; }

        public string URLCompetencia { get; set; }

        public List<ItemListaGestaoAvaliacaoColaboradorViewModel> ListaGestaoAvaliacaoColaboradorViewModel { get; set; }

        public List<ItemListaGestaoAvaliacaoPDIColaboradorViewModel> ListaGestaoAvaliacaoPDIColaboradorViewModel { get; set; }

        public int? AvaliacaoPDIColaboradorID { get; set; }

        public string DataCriacaoPDI { get; set; }

        public string StatusPDINome { get; set; }

        public bool ExibirObjetivosMetas { get; set; }

        public bool ExibirAutoAvaliacao { get; set; }

        public bool ExibirAvaliacaoGestor { get; set; }
    }

    public class ItemListaGestaoAvaliacaoColaboradorViewModel
    {
        public int AvaliacaoColaboradorID { get; set; }

        public string DataCriacao { get; set; }

        public string UsuarioNome { get; set; }

        public string Cargo { get; set; }

        public string StatusAvaliacaoColaboradorNome { get; set; }

        public int UsuarioID { get; set; }
    }

    public class ItemListaGestaoAvaliacaoPDIColaboradorViewModel
    {
        public int AvaliacaoPDIColaboradorID { get; set; }

        public string DataCriacao { get; set; }

        public string UsuarioNome { get; set; }

        public string Cargo { get; set; }

        public string StatusAvaliacaoPDIColaboradorNome { get; set; }

        public int UsuarioID { get; set; }
    }
}