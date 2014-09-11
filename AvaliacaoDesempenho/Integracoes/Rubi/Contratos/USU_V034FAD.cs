using System;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.Rubi.Contratos
{
    public class USU_V034FAD
    {
        [Key]
        public int USU_V034FADID { get; set; }
        public short NUMEMP { get; set; }

        public int NUMCAD { get; set; }

        public string NOMFUN { get; set; }

        public string EMACOM { get; set; }

        public string CODCAR { get; set; }

        public string TITRED { get; set; }

        public short? USU_CODDIR { get; set; }

        public string CODCCU { get; set; }

        public string NOMCCU { get; set; }

        public int? NUMLOC { get; set; }

        public string NOMLOC { get; set; }

        public short? USU_LD1EMP { get; set; }

        public short? USU_LD1TIP { get; set; }

        public int? USU_LD1CAD { get; set; }

        public string LD1NOM { get; set; }

        public DateTime? DATADM { get; set; }

        public byte[] FOTEMP { get; set; }
    }
}