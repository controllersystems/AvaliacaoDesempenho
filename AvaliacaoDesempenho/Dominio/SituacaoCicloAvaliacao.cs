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
    
    public partial class SituacaoCicloAvaliacao
    {
        public SituacaoCicloAvaliacao()
        {
            this.CicloAvaliacao = new HashSet<CicloAvaliacao>();
        }
    
        public int ID { get; set; }
        public string Nome { get; set; }
    
        public virtual ICollection<CicloAvaliacao> CicloAvaliacao { get; set; }
    }
}
