using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class PerformanceColaboradorDAO
    {
        public PerformanceColaborador Obter(int avaliacaoColaborador_ID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.PerformanceColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == avaliacaoColaborador_ID).FirstOrDefault();
            }
        }

        public void Editar(PerformanceColaborador performanceColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(performanceColaborador).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Incluir(PerformanceColaborador performanceColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.PerformanceColaborador.Add(performanceColaborador);
                db.SaveChanges();
            }
        }
    }
}