using AutoMapper;
using System;
using System.Globalization;
using System.Threading;

namespace AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento
{
    public class NullableDateTimeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(ResolutionContext context)
        {
            var sourceDate = context.SourceValue as DateTime?;
            string data;
            if (sourceDate.HasValue)
                data = string.Format("{0}/{1}/{2}", sourceDate.Value.Day.ToString("0#"), sourceDate.Value.Month.ToString("0#"), sourceDate.Value.Year.ToString("000#"));
            else
                data = string.Empty;
            return data;
        }
    }

    public class DateTimeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(ResolutionContext context)
        {
            var sourceDate = context.SourceValue as DateTime?;
            string data;
            if (sourceDate.HasValue)
                data = string.Format("{0}/{1}/{2}", sourceDate.Value.Day.ToString("0#"), sourceDate.Value.Month.ToString("0#"), sourceDate.Value.Year.ToString("000#"));
            else
                data = string.Empty;
            return data;
        }
    }
}