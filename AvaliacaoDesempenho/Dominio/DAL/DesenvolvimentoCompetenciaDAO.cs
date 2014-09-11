using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class DesenvolvimentoCompetenciaDAO
    {
        public List<DesenvolvimentoCompetencia> Listar(int idAvaliacaoPDI)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.DesenvolvimentoCompetencia
                            .Where(p => p.AvaliacaoPDIColaborador_ID == idAvaliacaoPDI).ToList();
            }
        }

        public DesenvolvimentoCompetencia Obter(int desenvolvimentoCompetenciaID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.DesenvolvimentoCompetencia
                            .Where(p => p.ID == desenvolvimentoCompetenciaID).FirstOrDefault();
            }
        }

        public void Incluir(DesenvolvimentoCompetencia desenvolvimentoCompetencia)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.DesenvolvimentoCompetencia.Add(desenvolvimentoCompetencia);
                db.SaveChanges();
            }
        }

        public void Editar(DesenvolvimentoCompetencia desenvolvimentoCompetencia)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(desenvolvimentoCompetencia).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Excluir(int desenvolvimentoCompentenciaID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var desenvolvimentoCompetencia = db.DesenvolvimentoCompetencia
                                                        .Where(p => p.ID == desenvolvimentoCompentenciaID).FirstOrDefault();

                if (desenvolvimentoCompetencia != null)
                {
                    db.DesenvolvimentoCompetencia.Remove(desenvolvimentoCompetencia);
                    db.SaveChanges();
                }
            }
        }
    }
}