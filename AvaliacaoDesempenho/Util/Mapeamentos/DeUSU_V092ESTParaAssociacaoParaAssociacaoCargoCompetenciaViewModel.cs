using AutoMapper;
using AvaliacaoDesempenho.Integracoes.Rubi.Contratos;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeUSU_V092ESTParaAssociacaoParaAssociacaoCargoCompetenciaViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<USU_V092EST, ItemListaGestaoCompetenciasCargosViewModel>()
                .ForMember(dest => dest.CargoRubiID, opt => opt.MapFrom(src => int.Parse(src.CODCAR)))
                .ForMember(dest => dest.CargoRubi, opt => opt.MapFrom(src => src.TITRED))
                .ForMember(dest => dest.AreaRubiID, opt => opt.MapFrom(src => int.Parse(src.CODCCU)))
                .ForMember(dest => dest.AreaRubi, opt => opt.MapFrom(src => src.NOMCCU))
                .ForMember(dest => dest.SetorRubiID, opt => opt.MapFrom(src => src.NUMLOC))
                .ForMember(dest => dest.SetorRubi, opt => opt.MapFrom(src => src.NOMLOC));
        }
    }
}