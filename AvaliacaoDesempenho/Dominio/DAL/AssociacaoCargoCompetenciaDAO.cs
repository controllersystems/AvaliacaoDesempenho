using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class AssociacaoCargoCompetenciaDAO
    {
        public List<AssociacaoCargoCompetencia> ListarPorCicloAvaliacao(int cicloAvaliacaoID)
        {
            List<AssociacaoCargoCompetencia> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AssociacaoCargoCompetencia
                                    .Where(p => p.CicloAvaliacao.ID == cicloAvaliacaoID);

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }

        public void PersistirColecao(List<AssociacaoCargoCompetencia> associacoesCargosCompetencias)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                 foreach (var item in associacoesCargosCompetencias)
                {
                    if (item.ID > 0)
                        db.Entry(item).State = EntityState.Modified;
                    else
                        db.AssociacaoCargoCompetencia.Add(item);
                }

                db.SaveChanges();
            }
        }

        public List<AssociacaoCargoCompetencia> Obter(int cicloAvaliacaoID, int? cargoRubiID, int? areaRubiID, int? setorRubiID)
        {
            List<AssociacaoCargoCompetencia> resultado = null;

            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                var query = db.AssociacaoCargoCompetencia
                                    .Where(p => p.CicloAvaliacao.ID == cicloAvaliacaoID);
                                             //&& (p.CargoRubiID = (cargoRubiID.HasValue ? cargoRubiID.Value : p.CargoRubiID)));

                if (query.Any())
                    resultado = query.ToList();
            }

            return resultado;
        }
    }
}