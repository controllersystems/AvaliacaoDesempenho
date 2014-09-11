using AutoMapper;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class Detbl_area_sccParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<tbl_area_scc, SelectListItem>()
                        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.id_area))
                        .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.titulo_area));
        }
    }
}