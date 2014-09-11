using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_cargo_competencia_scc
    {
        [Key]
        public int id_ccomp { get; set; }

        public tbl_cargo_scc cargo_ccomp { get; set; }

        public tbl_competencia_scc comp_ccomp { get; set; }

        public int? id_nivel_ccomp { get; set; }

        public tbl_competencia_proeficiencia_scc proeficiencia_ccomp { get; set; }
    }
}