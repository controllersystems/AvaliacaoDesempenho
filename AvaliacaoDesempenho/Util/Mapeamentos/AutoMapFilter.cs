using AutoMapper;
using System;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Util.Mappers
{
    public class AutoMapFilter : IActionFilter
    {
        private readonly Type _sourceType;
        private readonly Type _destType;

        public AutoMapFilter(Type sourceType, Type destType)
        {
            _sourceType = sourceType;
            _destType = destType;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;

            object viewModel = Mapper.Map(model, _sourceType, _destType);

            filterContext.Controller.ViewData.Model = viewModel;
        }


        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}