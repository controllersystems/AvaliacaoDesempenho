﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class AvaliacaoPDIColaboradorDAO
    {
        public AvaliacaoPDIColaborador Obter(int cicloAvaliacaoID, int usuarioID)
        {
            AvaliacaoPDIColaborador resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoPDIColaborador
                                    .Include("StatusPDI")
                                    .Include("DesenvolvimentoCompetencia")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID
                                                        && p.Colaborador_ID == usuarioID);

                if (query.Any())
                    resultado = query.First();
            }

            return resultado;
        }

        public void Incluir(AvaliacaoPDIColaborador avaliacaoPDIColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.AvaliacaoPDIColaborador.Add(avaliacaoPDIColaborador);
                db.SaveChanges();
            }
        }

        public void Editar(AvaliacaoPDIColaborador avaliacaoPDIColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(avaliacaoPDIColaborador).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<AvaliacaoPDIColaborador> ListarPorGestor(int cicloAvaliacaoID, int gestorRubiID, int gestorRubiEmpId)
        {
            List<AvaliacaoPDIColaborador> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoPDIColaborador
                                    .Include("StatusPDI")
                                    .Include("Usuario")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID
                                                && p.GestorRubi_ID == gestorRubiID
                                                && p.GestorRubiEmp_ID == gestorRubiEmpId);

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }

        public List<AvaliacaoPDIColaborador> Listar(int cicloAvaliacaoID)
        {
            List<AvaliacaoPDIColaborador> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoPDIColaborador
                                    .Include("StatusPDI")
                                    .Include("Usuario")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID);

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }
    }
}