﻿using AvaliacaoDesempenho.Dominio.DAL;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias;
using AvaliacaoDesempenho.Rubi.Integracoes;
using System;
using System.Web;

namespace AvaliacaoDesempenho.Seguranca
{
    public class Identidade
    {
        public int UsuarioID { get; private set; }

        public int? UsuarioRubiID { get; set; }

        public int PapelID { get; set; }

        public string NomePapel { get; set; }

        public string Login { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string DataAdmissao { get; private set; }

        public string DataCargo { get; private set; }

        public string Cargo { get; private set; }

        public int CodigoEmpresaRubiUD { get; set; }

        public string TempoCargo
        {
            get
            {
                DateTime dataCargo;

                if (DateTime.TryParse(DataCargo, out dataCargo))
                {
                    var diferencaAnosEmMeses = (DateTime.Today.Year - dataCargo.Year) * 12;
                    var diferencaMeses = (DateTime.Today.Month - dataCargo.Month);
                    var diferencaCompletudeMes = DateTime.Today.Day < dataCargo.Day ? -1 : 0;

                    var numeroMeses = diferencaAnosEmMeses + diferencaMeses + diferencaCompletudeMes;

                    return string.Format("{0} meses", numeroMeses);
                }
                else
                    return "?????";
            }
        }

        public int? GestorRubiID { get; set; }

        public string NomeGestor { get; private set; }

        public string Diretoria { get; private set; }

        public string Area { get; private set; }

        public string Localidade { get; private set; }

        public bool EhValido { get; private set; }

        public int? CargoRubiID { get; set; }

        public int? AreaRubiID { get; set; }

        public int? SetorRubiID { get; set; }

        public byte[] FOTEMP { get; set; }

        public Identidade()
        {
            Login = HttpContext.Current.User.Identity.Name;

            CarregarInformacoesUsuario();
        }

        public Identidade(int usuarioID)
        {
            Login = new UsuarioDAO().Obter(usuarioID).Login;

            CarregarInformacoesUsuario();
        }

        private void CarregarInformacoesUsuario()
        {
            CarregarInformacoesUsuarioAvaliacaoDesempenho();

            CarregarInformacoesUsuarioRubi();

            CarregarInformacoesUsuarioSistemaCompetencias();
        }

        private void CarregarInformacoesUsuarioAvaliacaoDesempenho()
        {
            var usuario = new UsuarioDAO().Obter(Login);

            if (usuario != null)
            {
                UsuarioID = usuario.ID;
                Nome = usuario.Nome;
                Email = usuario.Email;
                PapelID = usuario.Papel.ID;
                NomePapel = usuario.Papel.Nome;
                UsuarioRubiID = usuario.UsuarioRubiID;
                GestorRubiID = usuario.GestorRubiID;
                CodigoEmpresaRubiUD = usuario.CodigoEmpresaRubiUD;
            }
        }

        private void CarregarInformacoesUsuarioSistemaCompetencias()
        {
            var usuarioCompetencias = new IntegracaoSistemaCompetencias().ObterUsuarioCompetencias(Login);

            if (usuarioCompetencias != null)
            {
                Email = usuarioCompetencias.email_user;
                //Cargo = usuarioCompetencias.cargo_user;
                //Diretoria = "?????";
                //Localidade = "?????";

                if (usuarioCompetencias.id_area_user.HasValue)
                {
                    var areaCompetencias = new IntegracaoSistemaCompetencias().ObterAreaCompetencias(usuarioCompetencias.id_area_user.Value);

                    if (areaCompetencias != null)
                    {
                        Area = areaCompetencias.titulo_area;
                        EhValido = true;
                    }
                }
            }
        }

        private void CarregarInformacoesUsuarioRubi()
        {
            var informacoesRubi = new IntegracaoRubi().ObterUSU_V034FAD(CodigoEmpresaRubiUD, UsuarioRubiID.Value);

            if (informacoesRubi != null)
            {
                NomeGestor = informacoesRubi.LD1NOM;
                DataAdmissao = informacoesRubi.DATADM.HasValue ? informacoesRubi.DATADM.Value.ToShortDateString() : "?????";
                DataCargo = informacoesRubi.DATCAR.HasValue ? informacoesRubi.DATCAR.Value.ToShortDateString() : "?????";
                CargoRubiID = int.Parse(informacoesRubi.CODCAR);
                AreaRubiID = int.Parse(informacoesRubi.CODCCU);
                Diretoria = informacoesRubi.USU_CODDIR;
                Localidade = informacoesRubi.NOMLOC;
                FOTEMP = informacoesRubi.FOTEMP;
                Cargo = informacoesRubi.TITRED;
                
                if (informacoesRubi.NUMLOC.HasValue)
                    SetorRubiID = informacoesRubi.NUMLOC.Value;
            }
            else
                EhValido = false;
        }
    }
}