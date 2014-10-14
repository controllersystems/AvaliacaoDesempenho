using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorCompetenciasViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public DateTime? DataInicioAutoAvaliacao { get; set; }

        public DateTime? DataTerminoAutoAvaliacao { get; set; }

        public int? GestorRubiID { get; set; }

        public int? GestorRubiEmpID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int? StatusCicloAvaliacaoID { get; set; }

        public bool LiberadoPraSubmeterAoGestor { get; set; }

        public IEnumerable<SelectListItem> ListaNivelAvaliacao { get; set; }

        public List<ItemListaCompetenciasColaborador> ListaCompetenciasCorporativas { get; set; }

        public List<ItemListaCompetenciasColaborador> ListaCompetenciasLideranca { get; set; }

        public List<ItemListaCompetenciasColaborador> ListaCompetenciasFuncionais { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaCompetenciasColaborador
    {
        public int? ID { get; set; }

        public int CompentenciaID { get; set; }

        public string Competencia { get; set; }

        [Display(Name = "Nível colaborador")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public int? NivelColaborador { get; set; }
    }
}