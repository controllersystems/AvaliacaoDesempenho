using AvaliacaoDesempenho.Integracoes.Rubi.ContextoDados;
using AvaliacaoDesempenho.Integracoes.Rubi.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaliacaoDesempenho.Rubi.Integracoes
{
    public class IntegracaoRubi : IDisposable
    {
        internal RubiContext db;

        public List<USU_V034FAD> ListarUSU_V034FAD()
        {
            using (db = new RubiContext())
            {
                //return db.Database.SqlQuery<USU_V034FAD>("SELECT CAST(ROW_NUMBER() OVER (ORDER BY NUMEMP, NUMCAD) AS INT) USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD").ToList();
                return db.Database.SqlQuery<USU_V034FAD>("SELECT ROWNUM USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD").ToList();
            }
        }

        public List<USU_V092EST> ListarUSU_V092EST()
        {
            using (db = new RubiContext())
            {
                //return db.Database.SqlQuery<USU_V092EST>("SELECT CAST(ROW_NUMBER() OVER (ORDER BY CODCAR,CODCCU,NUMLOC) AS INT) USU_V092ESTID,CODCAR,TITRED,CODCCU,NOMCCU,NUMLOC,NOMLOC FROM USU_V092EST").ToList();
                return db.Database.SqlQuery<USU_V092EST>("SELECT ROWNUM USU_V092ESTID,CODCAR,TITRED,CODCCU,NOMCCU,NUMLOC,NOMLOC FROM USU_V092EST").ToList();
            }
        }

        public USU_V034FAD ObterUSU_V034FADPorEmail(string email)
        {
            using (db = new RubiContext())
            {
                //return db.Database.SqlQuery<USU_V034FAD>(string.Format("SELECT CAST(ROW_NUMBER() OVER (ORDER BY NUMEMP, NUMCAD) AS INT) USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD WHERE EMACOM = '{0}'", email)).FirstOrDefault();
                return db.Database.SqlQuery<USU_V034FAD>(string.Format("SELECT ROWNUM USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD WHERE EMACOM = '{0}'", email)).FirstOrDefault();
            }
        }

        public USU_V034FAD ObterUSU_V034FAD(int numEmp, int numCad)
        {
            using (db = new RubiContext())
            {
                //return db.Database.SqlQuery<USU_V034FAD>(string.Format("SELECT CAST(ROW_NUMBER() OVER (ORDER BY NUMEMP, NUMCAD) AS INT) USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD WHERE NUMEMP = {0} AND NUMCAD = {1}", numEmp, numCad)).FirstOrDefault();
                return db.Database.SqlQuery<USU_V034FAD>(string.Format("SELECT ROWNUM USU_V034FADID, NUMEMP,NUMCAD,NOMFUN,EMACOM,CODCAR,TITRED,USU_CODDIR,CODCCU,NOMCCU,NUMLOC,NOMLOC,USU_LD1EMP,USU_LD1TIP,USU_LD1CAD,LD1NOM,DATADM,FOTEMP FROM USU_V034FAD WHERE NUMEMP = {0} AND NUMCAD = {1}", numEmp, numCad)).FirstOrDefault();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}