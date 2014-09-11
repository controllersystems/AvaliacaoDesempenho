using AvaliacaoDesempenho.Models.CiclosAvaliacao;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Dominio.Validators
{
    public class ValidacaoCicloAvaliacao : BaseValidator
    {
        public JsonResult ValidarDataVigenciaInicialMenorFinal(string teste)
        {
            //bool ehValido = ValidarDataInicialMenorFinal(model.CicloAvaliacao.DataInicioVigencia,
            //                                             model.CicloAvaliacao.DataFimVigencia);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarDataObjetivosMetasInicialMenorFinal(ManterCicloAvaliacaoViewModel model)
        {
            //bool ehValido = ValidarDataInicialMenorFinal(model.CicloAvaliacao.DataInicioObjetivosMetas,
            //                                             model.CicloAvaliacao.DataTerminoObjetivosMetas);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarDataAutoAvaliacaoInicialMenorFinal(ManterCicloAvaliacaoViewModel model)
        {
            //bool ehValido = ValidarDataInicialMenorFinal(model.CicloAvaliacao.DataInicioAutoAvaliacao,
            //                                             model.CicloAvaliacao.DataTerminoAutoAvaliacao);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarDataAvaliacaoGestorInicialMenorFinal(ManterCicloAvaliacaoViewModel model)
        {
            //bool ehValido = ValidarDataInicialMenorFinal(model.CicloAvaliacao.DataInicioAvaliacaoGestor,
            //                                             model.CicloAvaliacao.DataTerminoAvaliacaoGestor);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarDataAvaliacaoGerenciamentoPDI(ManterCicloAvaliacaoViewModel model)
        {
            //bool ehValido = ValidarDataInicialMenorFinal(model.CicloAvaliacao.DataInicioGerenciamentoPDI,
            //                                             model.CicloAvaliacao.DataTerminoGerenciamentoPDI);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}