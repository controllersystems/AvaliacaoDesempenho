using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Models.Usuarios;
using AvaliacaoDesempenho.Rubi.Integracoes;
using AvaliacaoDesempenho.Seguranca;
using AvaliacaoDesempenho.Util.Mapeamentos;
using AvaliacaoDesempenho.Util.Mapeamentos.CriacaoMapeamento;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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

            return View("~/Views/Usuarios/GestaoAdministradores.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [CriacaoMapeamento(typeof(DeUsuarioParaItemListaAdministradorViewModel))]
        public ActionResult GestaoAdministradores(GestaoAdministradoresViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Admnistradores != null)
                {
                    foreach (var item in model.Admnistradores)
                    {
                        var usuario = new UsuarioDAO().Obter(item.UsuarioRubiID, item.CodigoEmpresaRubiUD);

                        if (usuario != null)
                        {
                            if (item.Selecionado)
                            {
                                usuario.Papel_ID = (int)Enumeradores.CodigoPapeis.Colaborador;
                                new UsuarioDAO().Editar(usuario);
                            }
                        }
                    }
                }
            }

            model.Admnistradores =
                Mapper.Map<List<Usuario>, List<ItemListaAdministradorViewModel>>
                    (new UsuarioDAO().ListarPorPapel(Enumeradores.CodigoPapeis.Administrador));

            return View("~/Views/Usuarios/GestaoAdministradores.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        [CriacaoMapeamento(typeof(DeUsuarioParaItemListaAdministradorViewModel))]
        public ActionResult PesquisarUsuario(GestaoAdministradoresViewModel model)
        {
            if (string.IsNullOrEmpty(model.NomeUsuarioPesquisa))
            {
                ModelState.AddModelError("NomeUsuarioPesquisa", "O nome do usuário é obrigatório.");
            }
            if (ModelState.IsValid)
            {
                model.UsuariosPesquisados = new List<ItemListaAdministradorViewModel>();

                PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);

                DirectorySearcher directorySearcher = new DirectorySearcher(principalContext.ConnectedServer);

                directorySearcher.Filter = "(&(|(sAMAccountName=" + model.NomeUsuarioPesquisa + ")(name=" + model.NomeUsuarioPesquisa + "))" + System.Configuration.ConfigurationManager.ConnectionStrings["ADFilterConnectionString"].ConnectionString + ")";

                SearchResultCollection searchResult = directorySearcher.FindAll();

                for (int i = 0; i < searchResult.Count; i++)
                {
                    DirectoryEntry directoryEntry = searchResult[i].GetDirectoryEntry();
                    if (directoryEntry.Properties.Count > 0)
                    {
                        model.UsuariosPesquisados.Add(new ItemListaAdministradorViewModel
                        {
                            Nome = directoryEntry.Properties["name"][0].ToString(),
                            Login = directoryEntry.Properties["sAMAccountName"][0].ToString(),
                            CodigoEmpresaRubiUD = int.Parse(directoryEntry.Properties["company"][0].ToString()),
                            UsuarioRubiID = int.Parse(directoryEntry.Properties["department"][0].ToString())
                        });
                    }
                }
            }

            model.Admnistradores =
                Mapper.Map<List<Usuario>, List<ItemListaAdministradorViewModel>>
                    (new UsuarioDAO().ListarPorPapel(Enumeradores.CodigoPapeis.Administrador));

            return View("~/Views/Usuarios/GestaoAdministradores.cshtml", model);
        }

        private void CadastrarUsuario(string login, int numEmp, int numCad)
        {
            IntegracaoRubi integracaoRubi = new IntegracaoRubi();
            UsuarioDAO usuarioDAO = new UsuarioDAO();

            var usuarioRubi = integracaoRubi.ObterUSU_V034FAD(numEmp, numCad);

            var usuario = new Usuario()
            {
                Nome = usuarioRubi.NOMFUN,
                Email = usuarioRubi.EMACOM,
                Login = login,
                Papel_ID = (int)Enumeradores.CodigoPapeis.Colaborador,
                GestorRubiID = usuarioRubi.USU_LD1CAD,
                UsuarioRubiID = usuarioRubi.NUMCAD,
                CodigoEmpresaRubiUD = usuarioRubi.NUMEMP
            };

            usuarioDAO.Incluir(usuario);
        }

        [Authorize]
        [HttpGet]
        [CriacaoMapeamento(typeof(DeUsuarioParaItemListaAdministradorViewModel))]
        public ActionResult IncluirUsuario(string login, int codigoEmpresaRubi, int usuarioRubi)
        {
            if (ModelState.IsValid)
            {
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                var usuario = usuarioDAO.Obter(usuarioRubi, codigoEmpresaRubi);

                if (usuario == null)
                {
                    CadastrarUsuario(login, codigoEmpresaRubi, usuarioRubi);

                    usuario = usuarioDAO.Obter(usuarioRubi, codigoEmpresaRubi);
                }

                usuario.Papel_ID = (int)Enumeradores.CodigoPapeis.Administrador;

                usuarioDAO.Editar(usuario);
            }

            return GestaoAdministradores();
        }

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