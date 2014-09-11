using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_tipo_competencia_scc
    {
        [Key]
        public int id_tipo { get; set; }

        public string titulo_tipo { get; set; }

        public string status_tipo { get; set; }
    }
}