using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeAssociacaoCargoCompetenciaParaItemListaGestaoCompetenciasCargosViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<AssociacaoCargoCompetencia, ItemListaGestaoCompetenciasCargosViewModel>();
        }
    }
}