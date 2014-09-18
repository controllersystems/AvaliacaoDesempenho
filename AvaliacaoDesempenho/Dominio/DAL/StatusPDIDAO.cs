using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class StatusPDIDAO
    {
        public StatusPDI Obter(int id)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.StatusPDI.Where(x=> x.ID == id).First();
            }
        }
    }
}