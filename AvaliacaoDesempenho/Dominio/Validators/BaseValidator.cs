using System;
using System.Web.Mvc;
using System.Web.UI;

namespace AvaliacaoDesempenho.Dominio.Validators
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class BaseValidator : Controller
    {
        public bool ValidarDataInicialMenorFinal(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial.Equals(DateTime.MinValue) || dataFinal.Equals(DateTime.MinValue))
                return true;
            else
                return (dataInicial < dataFinal);
        }

        public bool ValidarDataInicialMenorFinal(DateTime? dataInicial, DateTime? dataFinal)
        {
            if (!dataInicial.HasValue || !dataFinal.HasValue)
                return true;
            else
                return ValidarDataInicialMenorFinal(dataInicial.Value, dataFinal.Value);
        }
    }
}