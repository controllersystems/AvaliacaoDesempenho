using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeStatusAvaliacaoColaboradorParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<StatusAvaliacaoColaborador, SelectListItem>()
                        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ID))
                        .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome));
        }
    }
}