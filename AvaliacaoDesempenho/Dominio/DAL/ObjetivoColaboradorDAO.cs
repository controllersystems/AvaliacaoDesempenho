using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class ObjetivoColaboradorDAO
    {
        public void Incluir(ObjetivoColaborador objetivo)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.ObjetivoColaborador.Add(objetivo);
                db.SaveChanges();
            }
        }

        public List<ObjetivoColaborador> Listar(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ObjetivoColaborador
                            .Include("MetaColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor")
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao).ToList();
            }
        }

        public bool ExisteObjetivoSemAvaliacaoGestor(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.ObjetivoColaborador
                            .Include("MetaColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor")
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && (p.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao == "" ||
                                       p.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor.Avaliacao == null));
                return query.Any();
            }
        }

        public bool ExisteObjetivoSemResultadoAtingido(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.ObjetivoColaborador
                            .Include("MetaColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador")
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && (p.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido == "" ||
                                       p.MetaColaborador.ResultadoAtingidoColaborador.ResultadoAtingido == null));
                return query.Any();
            }
        }

        public ObjetivoColaborador Obter(int objetivoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ObjetivoColaborador
                            .Include("MetaColaborador")
                            .Include("MetaColaborador.ResultadoAtingidoColaborador")
                            .Where(p => p.ID == objetivoColaboradorID).FirstOrDefault();
            }
        }

        public void Editar(ObjetivoColaborador objetivo)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                if (objetivo.MetaColaborador.ResultadoAtingidoColaborador != null)
                {
                    if (!objetivo.MetaColaborador.ResultadoAtingidoColaborador_ID.HasValue)
                        db.ResultadoAtingidoColaborador.Add(objetivo.MetaColaborador.ResultadoAtingidoColaborador);
                    else
                    {
                        if (!objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor_ID.HasValue)
                            db.AvaliacaoGestor.Add(objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor);
                        else
                            db.Entry(objetivo.MetaColaborador.ResultadoAtingidoColaborador.AvaliacaoGestor).State = EntityState.Modified;
                        db.Entry(objetivo.MetaColaborador.ResultadoAtingidoColaborador).State = EntityState.Modified;
                    }
                }

                db.Entry(objetivo.MetaColaborador).State = EntityState.Modified;
                db.Entry(objetivo).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Excluir(int objetivoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var objetivo = db.ObjetivoColaborador
                                    .Include("MetaColaborador")
                                    .Include("MetaColaborador.ResultadoAtingidoColaborador")
                                    .Where(p => p.ID == objetivoColaboradorID).FirstOrDefault();

                if (objetivo != null)
                {
                    db.ObjetivoColaborador.Remove(objetivo);
                    db.SaveChanges();
                }
            }
        }
    }
}