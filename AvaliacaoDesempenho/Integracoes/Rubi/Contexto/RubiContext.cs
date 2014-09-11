using AvaliacaoDesempenho.Integracoes.Rubi.Contratos;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AvaliacaoDesempenho.Integracoes.Rubi.ContextoDados
{
    public class RubiContext : DbContext
    {
        public RubiContext()
            : base("RubiContext")
        {
        }

        public DbSet<USU_V092EST> USU_V092EST { get; set; }

        public DbSet<USU_V034FAD> USU_V034FAD { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}