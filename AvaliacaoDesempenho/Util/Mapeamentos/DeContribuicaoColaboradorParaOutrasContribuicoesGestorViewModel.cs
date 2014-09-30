using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeContribuicaoColaboradorParaOutrasContribuicoesGestorViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ContribuicaoColaborador, OutrasContribuicoesGestorViewModel>()
                .ForMember(dest => dest.AvaliacaoGestor,
                           opt => opt.MapFrom(source => source.AvaliacaoGestor.Avaliacao));
        }
    }
}