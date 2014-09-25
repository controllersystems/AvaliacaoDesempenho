using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class RecomendacaoColaboradorDAO
    {
        public RecomendacaoColaborador Obter(int avaliacaoColaborador_ID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.RecomendacaoColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == avaliacaoColaborador_ID).FirstOrDefault();
            }
        }

        public void Incluir(RecomendacaoColaborador recomendacaoColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.RecomendacaoColaborador.Add(recomendacaoColaborador);
                db.SaveChanges();
            }
        }

        public void Editar(RecomendacaoColaborador recomendacaoColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(recomendacaoColaborador).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}