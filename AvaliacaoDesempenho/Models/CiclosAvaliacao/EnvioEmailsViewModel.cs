using AvaliacaoDesempenho.Dominio.BusinessEntities;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class EnvioEmailsViewModel
    {
        public int? CicloAvaliacaoID { get; set; }

        public string DescricaoCiclo { get; set; }

        public bool Gestores { get; set; }

        public EmailInformativo Email { get; set; }

        public bool VerEmail { get; set; }
    }
}