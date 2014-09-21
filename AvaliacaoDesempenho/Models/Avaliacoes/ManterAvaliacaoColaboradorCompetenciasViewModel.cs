using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorCompetenciasViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int StatusCicloAvaliacaoID { get; set; }

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

        public int NivelColaborador { get; set; }

        public int NivelRequerido { get; set; }

        public int NivelGestor { get; set; }

        public string ComentarioGestor { get; set; }
    }
}