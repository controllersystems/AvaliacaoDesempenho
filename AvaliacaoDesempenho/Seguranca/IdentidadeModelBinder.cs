using System;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Seguranca
{
    public class IdentidadeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            return new Identidade();
        }
    }
}