using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeCicloAvaliacaoParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<CicloAvaliacao, SelectListItem>()
                        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ID))
                        .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}