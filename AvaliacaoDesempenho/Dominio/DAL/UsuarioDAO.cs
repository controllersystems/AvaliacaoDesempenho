using System.Collections.Generic;
using System.Linq;

namespace AvaliacaoDesempenho.Dominio.DAL
{
    public class UsuarioDAO
    {
        public Usuario Obter(int usuarioID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.Usuario.Include("Papel").FirstOrDefault(p => p.ID.Equals(usuarioID));
            }
        }

        public Usuario Obter(int usuarioRubi, int empresaRubi)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.Usuario.Where(u => u.UsuarioRubiID == usuarioRubi
                                        && u.CodigoEmpresaRubiUD == empresaRubi).FirstOrDefault();
            }
        }

        public Usuario Obter(string login)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.Usuario.Include("Papel").FirstOrDefault(p => p.Login.Equals(login));
            }
        }

        public void Incluir(Usuario usuario)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
            }
        }

        public List<Usuario> ListarPorPapel(BusinessEntities.Enumeradores.CodigoPapeis papelID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return db.Usuario.Where(p => p.Papel_ID.Equals((int)papelID)).ToList();
            }
        }

        public List<Usuario> ListarGestoresPorCicloAvaliacao(int cicloAvaliacaoID)
        {
            using (var db = new AvaliacaoDesempenhoContextEntities())
            {
                return (from avaliacao in db.AvaliacaoColaborador
                        join gestor in db.Usuario
                            on avaliacao.GestorRubiID equals gestor.UsuarioRubiID
                        where avaliacao.CicloAvaliacao_ID == cicloAvaliacaoID
                        select gestor).ToList();
            }
        }
    }
}