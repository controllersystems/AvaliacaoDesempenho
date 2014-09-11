using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class ContribuicaoColaboradorDAO
    {
        public List<ContribuicaoColaborador> Listar(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ContribuicaoColaborador
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao).ToList();
            }
        }

        public ContribuicaoColaborador Obter(int contribuicaoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ContribuicaoColaborador
                            .Where(p => p.ID == contribuicaoColaboradorID).FirstOrDefault();
            }
        }

        public void Incluir(ContribuicaoColaborador contribuicao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.ContribuicaoColaborador.Add(contribuicao);
                db.SaveChanges();
            }
        }

        public void Editar(ContribuicaoColaborador contribuicao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(contribuicao).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Excluir(int contribuicaoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var contribuicao = db.ContribuicaoColaborador
                                        .Where(p => p.ID == contribuicaoColaboradorID).FirstOrDefault();

                if (contribuicao != null)
                {
                    db.ContribuicaoColaborador.Remove(contribuicao);
                    db.SaveChanges();
                }
            }
        }
    }
}