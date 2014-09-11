using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Integracoes.SistemaCompetencias.Contratos
{
    public class tbl_usuario_scc
    {
        [Key]
        public int id_user { get; set; }

        public string login_user { get; set; }

        public string nome_user { get; set; }

        public string empresa_user { get; set; }

        public string cargo_user { get; set; }

        public char status_user { get; set; }

        public char tipo_user { get; set; }

        public string email_user { get; set; }

        public string tel_user { get; set; }

        public string cel_user { get; set; }

        public int? id_setor_user { get; set; }

        public int? qtde_acessos_user { get; set; }

        public int? id_empresa_user { get; set; }

        public int? id_loja_user { get; set; }

        public int? matricula_user { get; set; }

        public int? id_area_user { get; set; }

        public int? id_cargo_user { get; set; }
    }
}