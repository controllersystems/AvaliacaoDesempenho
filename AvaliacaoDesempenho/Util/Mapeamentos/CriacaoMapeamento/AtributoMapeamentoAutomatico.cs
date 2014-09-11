using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CriacaoMapeamentoAttribute : ActionFilterAttribute
    {
        private readonly Type _tipo;

        public CriacaoMapeamentoAttribute(Type tipo)
        {
            _tipo = tipo;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_tipo.GetInterfaces().Contains(typeof(IMapeamento)))
            {
                var objeto = _tipo.GetConstructors().First().Invoke(new object[] { });

                ((IMapeamento)objeto).Configurar();
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}