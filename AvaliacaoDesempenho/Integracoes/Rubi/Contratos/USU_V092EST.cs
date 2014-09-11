using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.Rubi.Contratos
{
    public class USU_V092EST
    {
        [Key]
        public int USU_V092ESTID { get; set; }

        public string CODCAR { get; set; }

        public string TITRED { get; set; }

        public string CODCCU { get; set; }

        public string NOMCCU { get; set; }

        public int? NUMLOC { get; set; }

        public string NOMLOC { get; set; }
    }
}