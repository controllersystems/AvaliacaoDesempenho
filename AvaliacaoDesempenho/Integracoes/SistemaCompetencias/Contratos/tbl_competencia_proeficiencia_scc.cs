using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_competencia_proeficiencia_scc
    {
        [Key]
        public int id_cp { get; set; }

        public tbl_competencia_scc comp_cp { get; set; }

        public string proeficiencia_cp { get; set; }

        public string descricao_proeficiencia_cp { get; set; }
    }
}