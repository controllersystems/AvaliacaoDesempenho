using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_area_scc
    {
        [Key]
        public int id_area { get; set; }

        public string titulo_area { get; set; }

        public string status_area { get; set; }
    }
}