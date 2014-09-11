using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contexto;
using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias
{
    public class IntegracaoSistemaCompetencias : IDisposable
    {
        internal SistemaCompetenciasContext db;

        public List<tbl_cargo_scc> ListarCargosCompetencias()
        {
            using (db = new SistemaCompetenciasContext())
            {
                return db.tbl_cargo_scc.ToList();
            }
        }

        public List<tbl_area_scc> ListarAreasCompetencias()
        {
            using (db = new SistemaCompetenciasContext())
            {
                return db.tbl_area_scc.ToList();
            }
        }

        public tbl_area_scc ObterAreaCompetencias(int id)
        {
            using (db = new SistemaCompetenciasContext())
            {
                return db.tbl_area_scc.FirstOrDefault(p => p.id_area.Equals(id));
            }
        }

        public List<tbl_setor_scc> ListarSetoresCompetencias()
        {
            using (db = new SistemaCompetenciasContext())
            {
                return db.tbl_setor_scc.ToList();
            }
        }

        public List<tbl_competencia_scc> ListarCompentenciasCargo(int cargoID, int areaID, int setorID)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select distinct [id_comp],[titulo_comp],[id_area_comp],[status_comp],[sigla_comp],[id_setor_comp],[id_nivel_comp],[id_tipo_comp],[descricao_comp]");
            sb.Append("from tbl_cargo_competencia_scc cc,tbl_competencia_scc cp,tbl_cargo_scc c");
            sb.Append("where cc.id_comp_ccomp = cp.id_comp and cc.id_nivel_ccomp = cp.id_nivel_comp and cc.id_cargo_ccomp = c.id_cargo");
            sb.Append(string.Format("and cc.id_nivel_ccomp = c.id_nivel_cargo and c.id_cargo = {0} and c.id_area_cargo = {1} and c.id_setor_cargo = {2}", cargoID, areaID, setorID));

            using (db = new SistemaCompetenciasContext())
            {
                return db.Database.SqlQuery<tbl_competencia_scc>(sb.ToString()).ToList();
            }
        }

        public tbl_usuario_scc ObterUsuarioCompetencias(string login)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT id_user,login_user,senha_user,nome_user,empresa_user,cargo_user,status_user,");
            sb.Append("email_user,tel_user,cel_user,id_setor_user,qtde_acessos_user,id_empresa_user,");
            sb.Append("id_loja_user,matricula_user,id_area_user,id_cargo_user,tipo_user");
            sb.Append(" FROM tbl_usuario_scc ");
            sb.Append(string.Format(" WHERE login_user = '{0}' ", login));

            using (db = new SistemaCompetenciasContext())
            {
                return db.Database.SqlQuery<tbl_usuario_scc>(sb.ToString()).FirstOrDefault();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}