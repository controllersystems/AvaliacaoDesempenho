using System.Collections.Generic;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class StatusAvaliacaoColaboradorDAO
    {
        public StatusAvaliacaoColaborador Obter(int ID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.StatusAvaliacaoColaborador.First(p => p.ID == ID);
            }
        }

        public List<StatusAvaliacaoColaborador> Listar()
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.StatusAvaliacaoColaborador.ToList();
            }
        }
    }
}