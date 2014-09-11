using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeObjetivoColaboradorParaObjetivoMetaViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ObjetivoColaborador, ObjetivoMetaViewModel>();
        }
    }
}