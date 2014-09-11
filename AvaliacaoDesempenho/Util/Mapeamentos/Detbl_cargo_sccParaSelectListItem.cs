using AutoMapper;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class Detbl_cargo_sccParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<tbl_cargo_scc, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.id_cargo))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.titulo_cargo));
        }
    }
}