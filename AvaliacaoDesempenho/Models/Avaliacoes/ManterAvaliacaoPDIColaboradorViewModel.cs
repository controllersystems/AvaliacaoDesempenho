using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoPDIColaboradorViewModel
    {
        public int? ColaboradorID { get; set; }
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoPDIColaboradorID { get; set; }

        [Display(Name = "Idiomas")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string Idiomas { get; set; }

        [Display(Name = "Graduação (Ano / Curso / Nível / Instituição de Ensino)")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválido.")]
        [StringLength(400, ErrorMessage = "A {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string Graduacao { get; set; }

        [Display(Name = "Pontos fortes (competências)")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string PontosFortes { get; set; }

        [Display(Name = "Pontos para desenvolvimento (conhecimento, habilidades, competências, relacionamento interpessoal) - Cite 2 a 3")]
        [DataType(DataType.Text, ErrorMessage = "O Ponto para desenvolvimento é inválido.")]
        [StringLength(400, ErrorMessage = "O Ponto para desenvolvimento deve ter o tamanho máximo de 400 caracteres.")]
        public string PontosDesenvolvimento { get; set; }

        [Display(Name = "Comentários do Colaborador")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string ComentariosColaborador { get; set; }

        [Display(Name = "Comentários do Gestor")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(400, ErrorMessage = "O {0} deve ter o tamanho máximo de 400 caracteres.")]
        public string ComentariosGestor { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int? CargoRubiID { get; set; }

        public int? AreaRubiID { get; set; }

        public int? SetorRubiID { get; set; }

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

        [Display(Name="Ações de desenvolvimento para alavancar pontos fortes ou para desenvolver outras competências (atividades, projetos, feedback, coaching/mentoring, treinamento/estudo, leitura, etc.)")]
        [DataType(DataType.Text, ErrorMessage = "As ações de desenvolvimento é inválida.")]
        [StringLength(600, ErrorMessage = "A ação de desenvolvimento deve ter o tamanho máximo de 600 caracteres.")]
        public string AcaoDesenvolvimento { get; set; }

        [Display(Name="Resurso/suporte necessários para atingir o resultado")]
        [DataType(DataType.Text, ErrorMessage = "O Resurso/suporte é inválido.")]
        [StringLength(600, ErrorMessage = "O Resurso/suporte deve ter o tamanho máximo de 600 caracteres.")]
        public string RecursoSuporte { get; set; }
    }
}