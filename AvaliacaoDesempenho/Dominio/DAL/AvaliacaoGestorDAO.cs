using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class AvaliacaoGestorDAO
    {
        public void Incluir(AvaliacaoGestor avaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.AvaliacaoGestor.Add(avaliacao);
                db.SaveChanges();
            }

        }

        public List<AvaliacaoGestor> ListarPorAvaliacaoID(int avaliacaoID)
        {
            List<AvaliacaoGestor> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = (from o in db.ObjetivoColaborador
                             join m in db.MetaColaborador on o.MetaColaborador_ID equals m.ID 
                             join r in db.ResultadoAtingidoColaborador on m.ResultadoAtingidoColaborador_ID equals r.ID 
                             join g in db.AvaliacaoGestor on r.AvaliacaoGestor_ID equals g.ID
                             where o.AvaliacaoColaborador_ID == avaliacaoID
                             select g); 
                                    
                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }

        public void PersistirColecao(List<AvaliacaoGestor> AvaliacaoGestorColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                foreach (var item in AvaliacaoGestorColaborador)
                {
                    if (item.ID > 0)
                        db.Entry(item).State = EntityState.Modified;
                    else
                        db.AvaliacaoGestor.Add(item);
                }

                db.SaveChanges();
            }
        }
    }
}