using AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contexto
{
    public class SistemaCompetenciasContext : DbContext
    {
        public SistemaCompetenciasContext()
            : base("CompetenciasRHContext")
        {
        }

        public DbSet<tbl_cargo_scc> tbl_cargo_scc { get; set; }

        public DbSet<tbl_area_scc> tbl_area_scc { get; set; }

        public DbSet<tbl_setor_scc> tbl_setor_scc { get; set; }

        public DbSet<tbl_usuario_scc> tbl_usuario_scc { get; set; }

        public DbSet<tbl_competencia_scc> tbl_competencia_scc { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}