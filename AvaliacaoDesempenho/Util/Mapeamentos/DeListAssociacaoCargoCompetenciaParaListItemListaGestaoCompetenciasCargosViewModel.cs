using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Collections.Generic;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeListAssociacaoCargoCompetenciaParaListItemListaGestaoCompetenciasCargosViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<List<AssociacaoCargoCompetencia>, List<ItemListaGestaoCompetenciasCargosViewModel>>();
        }
    }
}