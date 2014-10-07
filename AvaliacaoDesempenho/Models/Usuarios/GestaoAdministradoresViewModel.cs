using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Usuarios
{
    public class GestaoAdministradoresViewModel
    {
        public List<ItemListaAdministradorViewModel> Admnistradores { get; set; }

        [Display(Name = "Nome")]
        public string NomeUsuarioPesquisa { get; set; }

        public ItemListaAdministradorViewModel UsuarioPesquisado { get; set; }

        public List<ItemListaAdministradorViewModel> UsuariosPesquisados { get; set; }
    }

    public class ItemListaAdministradorViewModel
    {
        public int CodigoEmpresaRubiUD { get; set; }

        public int UsuarioRubiID { get; set; }

        public string Nome { get; set; }

        public string Login { get; set; }

        public bool Selecionado { get; set; }
    }
}