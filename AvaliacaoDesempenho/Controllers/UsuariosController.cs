using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Models.Usuarios;
using AvaliacaoDesempenho.Seguranca;
using AvaliacaoDesempenho.Util.Mapeamentos;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AvaliacaoDesempenho.Controllers
{
    public class UsuariosController : Controller
    {
        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeUsuarioParaItemListaAdministradorViewModel))]
        public ActionResult GestaoAdministradores()
        {
            GestaoAdministradoresViewModel model = new GestaoAdministradoresViewModel();

            model.Admnistradores =
                Mapper.Map<List<Usuario>, List<ItemListaAdministradorViewModel>>
                    (new UsuarioDAO().ListarPorPapel(Enumeradores.CodigoPapeis.Administrador));

            return View(model);
        }

        //[Authorize]
        //[HttpPost]
        //[CriacaoMapeamento(typeof(DeUsuarioParaItemListaAdministradorViewModel))]
        //public ActionResult GestaoAdministradores()
        //{
        //    GestaoAdministradoresViewModel model = new GestaoAdministradoresViewModel();

        //    model.Admnistradores =
        //        Mapper.Map<List<Usuario>, List<ItemListaAdministradorViewModel>>
        //            (new UsuarioDAO().Obter(Enumeradores.CodigoPapeis.Administrador));

        //    return View(model);
        //}




        //PesquisarUsuario

        #region Views Parciais

        [Authorize]
        [CriacaoMapeamento(typeof(DeIdentidadeParaCabecalhoColaboradorViewModel))]
        public ActionResult CabecalhoColaborador(int? cicloAvaliacaoSelecionadoID, int? colaboradorID = null)
        {
            Identidade identidade;

            if (!colaboradorID.HasValue)
                identidade = new Identidade();
            else
                identidade = new Identidade(colaboradorID.Value);

            CabecalhoColaboradorViewModel model =
                Mapper.Map<Identidade, CabecalhoColaboradorViewModel>(identidade);

            model.CicloAvaliacaoDescricao = new CicloAvaliacaoDAO().Obter(cicloAvaliacaoSelecionadoID.Value).Descricao;

            return PartialView("~/Views/Avaliacoes/CabecalhoColaborador.cshtml", model);
        }

        #endregion Views Parciais
    }
}