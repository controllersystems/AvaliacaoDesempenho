using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class GapCompetenciasViewModel
    {
        public int AnoReferencia { get; set; }
        public List<ItemGapCompetenciasViewModel> ListaGapCompetencias { get; set; }
    }
    public class ItemGapCompetenciasViewModel
    {
        public string Diretoria { get; set; }
        public string Area { get; set; }
        public string Gestor { get; set; }
        public string NomeColaborador { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string TipoCompetencia { get; set; }
        public string NomeCompetencia { get; set; }
        public int NivelRequirido  {get; set;}
        public int NivelAvaliadoGestor { get; set; }
        public int CampoGap { get; set; }
    }
}