﻿using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Models.Autenticacao;
using AvaliacaoDesempenho.Rubi.Integracoes;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using System.Web.Security;

namespace AvaliacaoDesempenho.Controllers
{
    public class AutenticacaoController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View("/Views/Autenticacao/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string URLRetorno)
        {
            if (ModelState.IsValid)
            {
                //MembershipUser usuario = Membership.GetUser(model.Login);

                //if (usuario != null && Membership.ValidateUser(model.Login, model.Senha))
                //{
                //    FormsAuthentication.SetAuthCookie(model.Login, false);

                //    if (!VerificarExistenciaUsuario(model.Login))
                //    {
                //        PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);

                //        DirectorySearcher directorySearcher = new DirectorySearcher(principalContext.ConnectedServer);

                //        directorySearcher.Filter = "(&(sAMAccountName=" + model.Login + ")" + System.Configuration.ConfigurationManager.ConnectionStrings["ADFilterConnectionString"].ConnectionString + ")";

                //        SearchResult searchResult = directorySearcher.FindOne();

                //        DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();

                //        if (directoryEntry.Properties.Count > 0)
                //        {
                //            int numEmp = int.Parse(directoryEntry.Properties["company"][0].ToString());
                //            int numCad = int.Parse(directoryEntry.Properties["department"][0].ToString());

                //            CadastrarUsuario(model.Login, numEmp, numCad);
                //        }
                //        else
                //        {
                //            model.Validado = false;
                //        }
                //    }

                //    if (Url.IsLocalUrl(URLRetorno)
                //        && URLRetorno.Length > 1
                //        && URLRetorno.StartsWith("/")
                //        && !URLRetorno.StartsWith("//")
                //        && !URLRetorno.StartsWith("/\\"))
                //    {
                //        return Redirect(URLRetorno);
                //    }
                //    else
                //    {
                //        return RedirectToAction("Index", "Home");
                //    }
                //}
                //else
                //{
                //    model.Validado = false;
                //}

                // BYPASS AD
                if (VerificarExistenciaUsuario(model.Login))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, false);

                    if (Url.IsLocalUrl(URLRetorno)
                        && URLRetorno.Length > 1
                        && URLRetorno.StartsWith("/")
                        && !URLRetorno.StartsWith("//")
                        && !URLRetorno.StartsWith("/\\"))
                    {
                        return Redirect(URLRetorno);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Autenticacao");
        }

        private bool VerificarExistenciaUsuario(string login)
        {
            return new UsuarioDAO().Obter(login) != null;
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
    }
}