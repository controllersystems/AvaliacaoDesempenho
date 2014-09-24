using System.Collections.Generic;
using System;
using System.Web;

namespace AvaliacaoDesempenho.Models.Avaliacoes
{
    public class ManterAvaliacaoColaboradorPerformanceViewModel
    {
        public int? ColaboradorID { get; set; }

        public int? CicloAvaliacaoSelecionadoID { get; set; }

        public int? AvaliacaoColaboradorID { get; set; }

        public int? GestorRubiID { get; set; }

        public int? UsuarioRubiID { get; set; }

        public int StatusAvaliacaoColaboradorID { get; set; }

        public int StatusCicloAvaliacaoID { get; set; }

        public ItemListaPerformanceColaborador AvaliacaoPerformanceGerais { get; set; }

        public bool AcessoGestor
        {
            get
            {
                return GestorRubiID.HasValue && UsuarioRubiID.HasValue && GestorRubiID.Value.Equals(UsuarioRubiID.Value);
            }
        }
    }

    public class ItemListaPerformanceColaborador
    {
        public int? ID { get; set; }

        public int CompentenciaID { get; set; }

        public string Competencia { get; set; }

        public string AvaliacaoPerformanceGeral { get; set; }
    }

}
