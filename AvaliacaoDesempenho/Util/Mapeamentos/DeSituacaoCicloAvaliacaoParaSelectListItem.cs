using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeSituacaoCicloAvaliacaoParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<SituacaoCicloAvaliacao, SelectListItem>()
                        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ID))
                        .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}