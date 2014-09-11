using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class AvaliacaoColaboradorDAO
    {
        public AvaliacaoColaborador Obter(int avaliacaoColaboradorID)
        {
            AvaliacaoColaborador resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoColaborador
                                    .Where(p => p.ID == avaliacaoColaboradorID);

                if (query.Any())
                    resultado = query.First();
            }

            return resultado;
        }

        public AvaliacaoColaborador Obter(int cicloAvaliacaoID, int usuarioID)
        {
            AvaliacaoColaborador resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoColaborador
                                    .Include("StatusAvaliacaoColaborador")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID
                                                        && p.Colaborador_ID == usuarioID);

                if (query.Any())
                    resultado = query.First();
            }

            return resultado;
        }

        public List<AvaliacaoColaborador> ListarPorGestor(int cicloAvaliacaoID, int gestorRubiID)
        {
            List<AvaliacaoColaborador> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoColaborador
                                    .Include("StatusAvaliacaoColaborador")
                                    .Include("Usuario")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID
                                                        && p.GestorRubiID == gestorRubiID);

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }

        public List<AvaliacaoColaborador> Listar(int cicloAvaliacaoID)
        {
            List<AvaliacaoColaborador> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AvaliacaoColaborador
                                    .Include("StatusAvaliacaoColaborador")
                                    .Include("Usuario")
                                    .Where(p => p.CicloAvaliacao_ID == cicloAvaliacaoID);

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }

        public void Incluir(AvaliacaoColaborador avaliacaoColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.AvaliacaoColaborador.Add(avaliacaoColaborador);
                db.SaveChanges();
            }
        }

        public void Editar(AvaliacaoColaborador avaliacaoColaborador)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Entry(avaliacaoColaborador).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}