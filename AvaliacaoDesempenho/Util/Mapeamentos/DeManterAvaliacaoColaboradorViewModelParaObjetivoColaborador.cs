using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeManterAvaliacaoColaboradorViewModelParaObjetivoColaborador : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ObjetivoMetaViewModel, ObjetivoColaborador>();
        }
    }
}