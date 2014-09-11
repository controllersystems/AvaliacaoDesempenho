//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvaliacaoDesempenho.Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class AvaliacaoPDIColaborador
    {
        public AvaliacaoPDIColaborador()
        {
            this.DesenvolvimentoCompetencia = new HashSet<DesenvolvimentoCompetencia>();
        }
    
        public int ID { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public string Idiomas { get; set; }
        public string Graduacao { get; set; }
        public string PontosFortes { get; set; }
        public string PontosDesenvolvimento { get; set; }
        public string ComentariosColaborador { get; set; }
        public string ComentariosGestor { get; set; }
        public bool Aprovada { get; set; }
        public int CicloAvaliacao_ID { get; set; }
        public int StatusPDI_ID { get; set; }
        public int Colaborador_ID { get; set; }
        public Nullable<int> GestorRubiID { get; set; }
    
        public virtual CicloAvaliacao CicloAvaliacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual StatusPDI StatusPDI { get; set; }
        public virtual ICollection<DesenvolvimentoCompetencia> DesenvolvimentoCompetencia { get; set; }
    }
}