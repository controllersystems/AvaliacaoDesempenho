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
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            var sourceDate = context.SourceValue as DateTime?;
            string data;
            if (sourceDate.HasValue)
                data = sourceDate.Value.ToShortDateString();
            else
                data = string.Empty;
            Thread.CurrentThread.CurrentCulture = originalCulture;
            return data;
        }
    }

    public class DateTimeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(ResolutionContext context)
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            var sourceDate = context.SourceValue as DateTime?;
            string data;
            if (sourceDate.HasValue)
                data = sourceDate.Value.ToShortDateString();
            else
                data = string.Empty;
            Thread.CurrentThread.CurrentCulture = originalCulture;
            return data;
        }
    }
}