using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeCicloAvaliacaoParaManterCicloAvaliacaoViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<DateTime?, string>().ConvertUsing<NullableDateTimeConverter>();
            Mapper.CreateMap<DateTime, string>().ConvertUsing<DateTimeConverter>();

            Mapper.CreateMap<CicloAvaliacao, ManterCicloAvaliacaoViewModel>();
        }
    }
}