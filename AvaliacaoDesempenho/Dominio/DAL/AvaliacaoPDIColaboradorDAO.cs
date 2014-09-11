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
    }
}