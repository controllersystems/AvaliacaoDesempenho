using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeDesenvolvimentoCompetenciaParaItemListaDesenvolvimentoCompetenciaViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<DesenvolvimentoCompetencia, ItemListaDesenvolvimentoCompetenciaViewModel>();
        }
    }
}