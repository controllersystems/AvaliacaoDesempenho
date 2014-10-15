using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeAvaliacaoGestorParaAvaliacaoGestorContribuinte : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<AvaliacaoGestor, AvaliacaoGestorContribuinte>()
                  .ForMember(dest => dest.ID, opts => opts.MapFrom(src => src.ID))
                  .ForMember(dest => dest.Avaliacao, opts => opts.MapFrom(src => src.Avaliacao));
        }
    }
}