using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Collections.Generic;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeListaItemListaGestaoCompetenciasCargosViewModelParaListaAssociacaoCargoCompetencia : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<List<ItemListaGestaoCompetenciasCargosViewModel>, List<AssociacaoCargoCompetencia>>();
        }
    }
}