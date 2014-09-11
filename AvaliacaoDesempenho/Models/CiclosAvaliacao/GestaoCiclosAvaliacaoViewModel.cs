using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class GestaoCiclosAvaliacaoViewModel
    {
        public List<ItemListaGestaoCicloAvaliacaoViewModel> CiclosAvaliacao { get; set; }
    }

    public class ItemListaGestaoCicloAvaliacaoViewModel
    {
        [Key]
        public int ID { get; set; }

        public string Descricao { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicioVigencia { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataFimVigencia { get; set; }

        public string SituacaoCicloAvaliacaoNome { get; set; }
    }
}