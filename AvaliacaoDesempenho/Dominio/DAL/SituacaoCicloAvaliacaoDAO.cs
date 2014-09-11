using System.Collections.Generic;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class SituacaoCicloAvaliacaoDAO
    {
        public List<SituacaoCicloAvaliacao> Listar()
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.SituacaoCicloAvaliacao.ToList();
            }
        }
    }
}