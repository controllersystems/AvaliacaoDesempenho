using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_competencia_scc
    {
        [Key]
        public int id_comp { get; set; }

        public string titulo_comp { get; set; }

        public int id_area_comp { get; set; }

        public char status_comp { get; set; }

        public string sigla_comp { get; set; }

        public int? id_setor_comp { get; set; }

        public int? id_nivel_comp { get; set; }

        public int? id_tipo_comp { get; set; }

        public string proeficiencia_cp { get; set; }

        public string descricao_comp { get; set; }
    }
}