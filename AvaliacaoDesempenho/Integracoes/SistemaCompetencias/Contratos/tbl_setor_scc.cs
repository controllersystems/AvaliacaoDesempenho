using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_setor_scc
    {
        [Key]
        public int id_setor { get; set; }

        public string titulo_setor { get; set; }

        public string status_setor { get; set; }

        public int id_area_setor { get; set; }
    }
}