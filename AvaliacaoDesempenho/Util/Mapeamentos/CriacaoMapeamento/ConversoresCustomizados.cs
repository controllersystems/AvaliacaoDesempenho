using AutoMapper;
using System;

namespace AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento
{
    public class NullableDateTimeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(ResolutionContext context)
        {
            var sourceDate = context.SourceValue as DateTime?;
            if (sourceDate.HasValue)
                return sourceDate.Value.ToShortDateString();
            else
                return string.Empty;
        }
    }

    public class DateTimeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(ResolutionContext context)
        {
            var sourceDate = context.SourceValue as DateTime?;
            if (sourceDate.HasValue)
                return sourceDate.Value.ToShortDateString();
            else
                return string.Empty;
        }
    }
}