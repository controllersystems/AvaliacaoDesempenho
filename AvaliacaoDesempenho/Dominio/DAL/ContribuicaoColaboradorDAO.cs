﻿using System;
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
                return db.ContribuicaoColaborador.Include("AvaliacaoGestor")
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao).ToList();
            }
        }


        public ContribuicaoColaborador Obter(int contribuicaoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ContribuicaoColaborador.Include("AvaliacaoGestor")
                            .Where(p => p.ID == contribuicaoColaboradorID).FirstOrDefault();
            }
        }

        public void Incluir(ContribuicaoColaborador contribuicao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                if (contribuicao.AvaliacaoColaborador != null)
                {
                    db.Entry(contribuicao.AvaliacaoColaborador).State = EntityState.Modified;
                }
                db.ContribuicaoColaborador.Add(contribuicao);
                db.SaveChanges();
            }
        }

        public void Editar(ContribuicaoColaborador contribuicao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                if (!contribuicao.AvaliacaoGestor_ID.HasValue)
                    db.AvaliacaoGestor.Add(contribuicao.AvaliacaoGestor);
                else
                    db.Entry(contribuicao.AvaliacaoGestor).State = EntityState.Modified;
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

        public bool ExisteContribuicaoSemAvaliacaoGestor(int idAvaliacao)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.ContribuicaoColaborador
                            .Include("ContribuicaoColaborador.AvaliacaoGestor")
                            .Where(p => p.AvaliacaoColaborador_ID == idAvaliacao
                                   && (p.AvaliacaoGestor.Avaliacao == "" ||
                                       p.AvaliacaoGestor.Avaliacao == null));
                return query.Any();
            }
        }
    }
}