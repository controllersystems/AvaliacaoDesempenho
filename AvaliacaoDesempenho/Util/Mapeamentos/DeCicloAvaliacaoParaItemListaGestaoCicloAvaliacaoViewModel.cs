using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeCicloAvaliacaoParaItemListaGestaoCicloAvaliacaoViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<CicloAvaliacao, ItemListaGestaoCicloAvaliacaoViewModel>();
        }
    }
}