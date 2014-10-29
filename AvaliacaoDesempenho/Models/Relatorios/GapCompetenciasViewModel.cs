using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class GapCompetenciasViewModel
    {
        public int? CicloSelecionado { get; set; }
        public int AnoReferencia { get; set; }
        public List<ItemGapCompetenciasViewModel> ListaGapCompetencias { get; set; }
        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
        [Display(Name = "Colaborador")]
        public string ColaboradorPesquisado { get; set; }
        [Display(Name = "Cargo")]
        public string CargoPesquisado { get; set; }
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
        public int? NivelRequirido  {get; set;}
        public int? NivelAvaliadoGestor { get; set; }
        public int CampoGap { get; set; }
    }
}