using AutoMapper;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class Detbl_setor_sccParaSelectListItem : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<tbl_setor_scc, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.id_setor))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.titulo_setor));
        }
    }
}