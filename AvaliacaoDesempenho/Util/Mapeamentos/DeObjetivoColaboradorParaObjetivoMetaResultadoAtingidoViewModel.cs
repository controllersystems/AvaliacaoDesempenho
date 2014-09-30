using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeObjetivoColaboradorParaObjetivoMetaResultadoAtingidoViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ObjetivoColaborador, ObjetivoMetaResultadoAtingidoViewModel>();
        }
    }
}