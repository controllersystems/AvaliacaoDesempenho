using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Avaliacoes;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeAvaliacaoColaboradorParaGestaoAvaliacaoColaboradorViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<DateTime?, string>().ConvertUsing<NullableDateTimeConverter>();
            Mapper.CreateMap<DateTime, string>().ConvertUsing<DateTimeConverter>();

            Mapper.CreateMap<AvaliacaoColaborador, GestaoAvaliacaoColaboradorViewModel>();
        }
    }
}