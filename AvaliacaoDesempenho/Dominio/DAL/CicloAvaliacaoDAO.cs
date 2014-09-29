using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class CicloAvaliacaoDAO
    {
        public List<CicloAvaliacao> Listar()
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.CicloAvaliacao.Include("SituacaoCicloAvaliacao").ToList();
            }
        }

        public List<CicloAvaliacao> ListarCiclosDisponiveis()
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.CicloAvaliacao.Include("SituacaoCicloAvaliacao").Where(x => x.SituacaoCicloAvaliacao_ID > 1 && x.SituacaoCicloAvaliacao_ID < 7).ToList();
            }
        }

        public List<CicloAvaliacao> ListarCiclosDisponiveis(int cargoID, int areaID, int setorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var cicloAssociacao = from ciclo in db.CicloAvaliacao
                                      join associacao in db.AssociacaoCargoCompetencia
                                      on ciclo.ID equals associacao.CicloAvaliacao_ID
                                      where ciclo.SituacaoCicloAvaliacao_ID > 1 &&
                                            ciclo.SituacaoCicloAvaliacao_ID < 7 &&
                                            associacao.CargoRubiID == cargoID &&
                                            associacao.AreaRubiID == areaID &&
                                            associacao.SetorRubiID == setorID && 
                                            associacao.CargoCompetenciaID != null &&
                                            associacao.AreaCompetenciaID != null &&
                                            associacao.SetorCompetenciaID != null
                                      select ciclo;
                

                //var query = from ciclo in db.CicloAvaliacao
                //            join associacao in db.AssociacaoCargoCompetencia 
                //                on new {ciclo.ID} equals new {associacao.CicloAvaliacao_ID}
                //            //into cicloAssociacao
                //            //join usuario in db.Usuario
                //            //on new { cicloAssociacao.AreaRubiID, cicloAssociacao.CargoRubiID, cicloAssociacao.SetorRubiID} 
                //            //    equals { usuario. }
                //            where ciclo.SituacaoCicloAvaliacao_ID > 1 &&
                //                  ciclo.SituacaoCicloAvaliacao_ID < 7 && 
                //                  associacao.CargoRubiID == usuario.car

                //return db.CicloAvaliacao
                //         .Include("SituacaoCicloAvaliacao")
                //         .Include("AssociacaoCargoCompetencia")
                //         .Include("Usuario")
                //         .Where(x => x.SituacaoCicloAvaliacao_ID > 1 
                //                  && x.SituacaoCicloAvaliacao_ID < 7
                //                  && x.AssociacaoCargoCompetencia).ToList();

                return cicloAssociacao.ToList();
            }
        }

        public CicloAvaliacao Obter(int id)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.CicloAvaliacao.Include("SituacaoCicloAvaliacao").First(p => p.ID == id);
            }
        }

        public void Incluir(CicloAvaliacao cicloAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.CicloAvaliacao.Add(cicloAvaliacao);
                db.SaveChanges();
            }
        }

        public void Editar(CicloAvaliacao cicloAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(cicloAvaliacao).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Remover(int id)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.CicloAvaliacao.Remove(Obter(id));
                db.SaveChanges();
            }
        }
    }
}