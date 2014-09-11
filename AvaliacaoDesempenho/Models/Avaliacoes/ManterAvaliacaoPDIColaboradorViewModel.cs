using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoPDIColaboradorViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoPDIColaboradorID { get; set; }

        [Display(Name = "Idiomas")]
        public string Idiomas { get; set; }

        [Display(Name = "Graduação (Ano / Curso / Nível / Instituição de Ensino)")]
        public string Graduacao { get; set; }

        [Display(Name = "Pontos fortes (competências)")]
        public string PontosFortes { get; set; }

        [Display(Name = "Pontos para desenvolvimento (conhecimento, habilidades, competências, relacionamento interpessoal) - Cite 2 a 3")]
        public string PontosDesenvolvimento { get; set; }

        [Display(Name = "Comentários do Colaborador")]
        public string ComentariosColaborador { get; set; }

        [Display(Name = "Comentários do Gestor")]
        public string ComentariosGestor { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusPDIID { get; set; }

        public bool IncluirDesenvolvimentoCompetencia { get; set; }

        public bool Aprovar { get; set; }

        public bool Aprovada { get; set; }

        public ItemListaDesenvolvimentoCompetenciaViewModel DesenvolvimentoCompetenciaCadastro { get; set; }

        public List<ItemListaDesenvolvimentoCompetenciaViewModel> ListaDesenvolvimentoCompetenciaViewModel { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaDesenvolvimentoCompetenciaViewModel
    {
        public int ID { get; set; }

        [Display(Name="Ações de desenvolvimento para alavancar pontos fortes ou para desenvolver outras competências (atividades, projetos, feedback, coaching/mentoring, treinamento/estudo, leitura, etc.")]
        public string AcaoDesenvolvimento { get; set; }

        [Display(Name="Resurso/suporte necessários para atingir o resultado")]
        public string RecursoSuporte { get; set; }
    }
}