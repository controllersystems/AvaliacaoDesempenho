using AutoMapper;
using AvaliacaoDesempenho.Models.Usuarios;
using AvaliacaoDesempenho.Seguranca;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeIdentidadeParaCabecalhoColaboradorViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<Identidade, CabecalhoColaboradorViewModel>();
        }
    }
}