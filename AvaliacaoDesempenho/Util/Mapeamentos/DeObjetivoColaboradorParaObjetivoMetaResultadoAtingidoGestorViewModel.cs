using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoGestorViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ObjetivoColaborador, ObjetivoMetaResultadoAtingidoGestorViewModel>()
                .ForMember(dest => dest.AvaliacaoGestor,
                           opt => opt.MapFrom(source => source.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao));
        }
    }
}