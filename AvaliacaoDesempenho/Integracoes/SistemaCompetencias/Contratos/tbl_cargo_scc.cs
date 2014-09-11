using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_cargo_scc
    {
        [Key]
        public int id_cargo { get; set; }

        public string titulo_cargo { get; set; }

        public string status_cargo { get; set; }

        public int id_area_cargo { get; set; }

        public int id_setor_cargo { get; set; }

        public int? id_nivel_cargo { get; set; }
    }
}