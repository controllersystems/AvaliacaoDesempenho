using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeManterCicloAvaliacaoViewModelParaCicloAvaliacao : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<ManterCicloAvaliacaoViewModel, CicloAvaliacao>();
        }
    }
}