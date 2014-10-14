using System.Collections.Generic;
using System;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorPerformanceViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? PapelID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public DateTime? DataInicioAvaliacaoGestor { get; set; }

        public DateTime? DataTerminoAvaliacaoGestor { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int? StatusCicloAvaliacaoID { get; set; }

        public ItemListaPerformanceColaborador AvaliacaoPerformanceGerais { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaPerformanceColaborador
    {
        public int? ID { get; set; }

        public int CompentenciaID { get; set; }

        public string Competencia { get; set; }

        [Display(Name = "Avaliação da performance")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(600, ErrorMessage = "A {0} deve ter o tamanho máximo de 600 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string AvaliacaoPerformanceGeral { get; set; }
    }
}
