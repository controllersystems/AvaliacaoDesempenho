using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class CompetenciaColaboradorDAO
    {
        public List<CompetenciaColaborador> Listar(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.CompetenciaColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao).ToList();
            }
        }

        public CompetenciaColaborador Obter(int idAvaliacao, int competenciaID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.CompetenciaColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && p.CompetenciaID == competenciaID).FirstOrDefault();
            }
        }

        public bool ExisteCompetenciaSemAvaliacao(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.CompetenciaColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && p.NivelColaborador == null);
                return query.Any();
            }
        }

        public bool ExisteCompetenciaSemAvaliacaoGestor(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.CompetenciaColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && p.NivelGestor == null);
                return query.Any();
            }
        }

        public bool ExisteCompetenciaAvaliadaDiferenteSemComentarioGestor(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.CompetenciaColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && p.NivelGestor != p.NivelColaborador
                                   && string.IsNullOrEmpty(p.ComentariosGestor));
                return query.Any();
            }
        }

        public void PersistirColecao(List<CompetenciaColaborador> competenciasColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                foreach (var item in competenciasColaborador)
                {
                    if (item.ID > 0)
                        db.Entry(item).State = EntityState.Modified;
                    else
                        db.CompetenciaColaborador.Add(item);
                }

                db.SaveChanges();
            }
        }

        public void Persistir(CompetenciaColaborador competenciaColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                if (competenciaColaborador.ID > 0)
                    db.Entry(competenciaColaborador).State = EntityState.Modified;
                else
                    db.CompetenciaColaborador.Add(competenciaColaborador);

                db.SaveChanges();
            }
        }
    }
}