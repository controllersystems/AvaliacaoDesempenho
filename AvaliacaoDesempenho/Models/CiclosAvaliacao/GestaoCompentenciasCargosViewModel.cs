using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class GestaoCompentenciasCargosViewModel
    {
        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public List<ItemListaGestaoCompetenciasCargosViewModel> AssociacoesCargosCompetencias { get; set; }

        public List<SelectListItem> CargosCompentencia { get; set; }

        public List<SelectListItem> AreasCompentencia { get; set; }

        public List<SelectListItem> SetoresCompentencia { get; set; }

        public int Pagina { get; set; }
    }

    public class ItemListaGestaoCompetenciasCargosViewModel
    {
        public int ID { get; set; }

        public int CargoRubiID { get; set; }

        public string CargoRubi { get; set; }

        public int AreaRubiID { get; set; }

        public string AreaRubi { get; set; }

        public int? SetorRubiID { get; set; }

        public string SetorRubi { get; set; }

        public int? CargoCompetenciaID { get; set; }

        public string CargoCompetencia { get; set; }

        public int? AreaCompetenciaID { get; set; }

        public string AreaCompetencia { get; set; }

        public int? SetorCompetenciaID { get; set; }

        public string SetorCompetencia { get; set; }

        public int CicloAvaliacaoID { get; set; }
    }
}