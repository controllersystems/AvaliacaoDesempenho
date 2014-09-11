using System.Collections.Generic;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorObjetivosMetasViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public bool IncluirMeta { get; set; }

        public bool Aprovar { get; set; }

        public bool Aprovada { get; set; }

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

        public string Objetivo { get; set; }

        public string MetaColaboradorMeta { get; set; }
    }
}