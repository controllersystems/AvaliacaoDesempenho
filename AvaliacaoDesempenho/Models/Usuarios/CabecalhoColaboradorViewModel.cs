namespace AvaliacaoDesempenho.Models.Usuarios
{
    public class CabecalhoColaboradorViewModel
    {
        public string CicloAvaliacaoDescricao { get; set; }

        public int UsuarioID { get; private set; }

        public string Nome { get; private set; }

        public string Login { get; private set; }

        public string DataAdmissao { get; set; }

        public string Cargo { get; set; }

        public string TempoCargo { get; set; }

        public string NomeGestor { get; set; }

        public string Diretoria { get; set; }

        public string Area { get; set; }

        public string Localidade { get; set; }

        public byte[] FOTEMP { get; set; }
    }
}