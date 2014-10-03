using System;
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

        public List<CicloAvaliacao> ListarCiclosDisponiveis(int cargoID, int areaID, int setorID, int gestorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var cicloAssociacao = from ciclo in db.CicloAvaliacao
                                      join associacao in db.AssociacaoCargoCompetencia
                                      on ciclo.ID equals associacao.CicloAvaliacao_ID
                                      where ciclo.SituacaoCicloAvaliacao_ID > 1 &&
                                            associacao.CargoRubiID == cargoID &&
                                            associacao.AreaRubiID == areaID &&
                                            associacao.SetorRubiID == setorID && 
                                            associacao.CargoCompetenciaID != null &&
                                            associacao.AreaCompetenciaID != null &&
                                            associacao.SetorCompetenciaID != null
                                      select ciclo;

                var ciclosDeSubordinados = from ciclo in db.CicloAvaliacao
                                           join avaliacao in db.AvaliacaoColaborador
                                           on ciclo.ID equals avaliacao.CicloAvaliacao_ID
                                           join usuario in db.Usuario
                                           on avaliacao.Colaborador_ID equals usuario.ID
                                           where ciclo.SituacaoCicloAvaliacao_ID > 1 &&
                                                 usuario.GestorRubiID.Value == gestorID
                                           select ciclo;

                return cicloAssociacao.Union(ciclosDeSubordinados).Distinct().ToList();
            }
        }

        public List<CicloAvaliacao> ListarCiclosSobrepostosQuePossuemEssaData(DateTime data)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var ciclos = db.CicloAvaliacao.Where(x => x.DataInicioVigencia <= data
                                                              && x.DataFimVigencia >= data);

                return ciclos.ToList();
            }
        }

        public List<CicloAvaliacao> ListarCiclosSobrepostosQuePossuemEssaData(DateTime data, int cicloExcluido)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var ciclos = db.CicloAvaliacao.Where(x => x.DataInicioVigencia <= data
                                                              && x.DataFimVigencia >= data
                                                              && x.ID != cicloExcluido);

                return ciclos.ToList();
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