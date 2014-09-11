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