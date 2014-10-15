using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class ResultadoAtingidoColaboradorDAO
    {
        public ResultadoAtingidoColaborador Obter(int ResultadoAtingidoColaboradorID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.ResultadoAtingidoColaborador
                            .Where(p => p.ID == ResultadoAtingidoColaboradorID).FirstOrDefault();
            }
        }
    }
}